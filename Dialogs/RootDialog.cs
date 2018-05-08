namespace StupidBotMessengerMultiDialogs.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;
    using MultiDialogsBot.Utils;
    using StupidBotMessengerMultiDialogs.Model;
    using StupidBotMessengerMultiDialogs.Services;

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string Booking = "Đặt phòng";

        private const string Asking = "Tra cứu";

        private const string Help = "Giúp đỡ";

        private Customer customer;
        private Reservation reservation;
        private Room room;

        /// <summary>
        /// Ở constructor, khởi tạo 2 biến thành viên là customer và reservation.
        /// </summary>
        public RootDialog()
        {
            customer = new Customer();
            reservation = new Reservation();
        }

        public async Task StartAsync(IDialogContext context)
        {
             context.Wait(this.MessageReceivedAsync);
        }


        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text.ToLower().Contains("help"))
            {
                context.Call(new SupportDialog(), this.ResumeAfterSupportDialog);
                await context.PostAsync("Vui lòng nhập vấn đề của bạn");
            }
            //Khi Khách hàng muốn đặt phòng, tạo một FormDialog chứa các field cho Reservation
            else if(message.Text.ToLower().Contains("đặt phòng"))
            {
                var reservationDialog = new FormDialog<Reservation>(this.reservation, Reservation.BuildOrderForm, FormOptions.PromptInStart);
                context.Call(reservationDialog, this.ResumeAfterReservationDialog);
            }
            else
            {
                this.ShowOptions(context);
            }
        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, 
                this.OnOptionSelected, new List<string>() { Booking, Asking, Help }, 
                "Bạn muốn đặt phòng hay tra cứu thông tin?",
                "Vui lòng chọn lại", 3);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case Asking:
                        context.Call(new LuisDialog(), this.ResumeAfterOptionDialog);
                        await context.PostAsync("Vui lòng nhập thông tin cần tra cứu");
                        break;

                    case Booking:
                        var reservationDialog = new FormDialog<Reservation>(this.reservation, Reservation.BuildOrderForm, FormOptions.PromptInStart);
                        context.Call(reservationDialog, this.ResumeAfterReservationDialog);
                        //context.Call(new RoomCategoryDialog(), this.ResumeAfterRoomCategoryDialog);
                        break;

                    case Help:
                        context.Call(new SupportDialog(), this.ResumeAfterSupportDialog);
                        await context.PostAsync("Vui lòng nhập vấn đề của bạn");
                        break;
                }
             
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Quá nhiều người hỏi. Bot đang bị stress");

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
        {
            await context.PostAsync($"Cảm ơn quý khách.");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
        //Sau khi khách chọn loại phòng, khách được chọn tiếp phòng tương ứng của loại phòng
        //Chọn loại phòng xong thì quay trở lại hàm này.
        //Ở đây, ta lưu thông tin về RoomID vào reseration.
        //Sau khi có các thông tin cần thiết, ta lấy thông tin khách hàng.
        private async Task ResumeAfterRoomCategoryDialog(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            this.reservation.RoomID = Convert.ToInt32(message);
            //Tạo ReceiptCard để hỏi xem thông tin đã đúng chưa
            using (RoomService roomservice = new RoomService())
            {
                await context.PostAsync("Thông tin phòng:");
                this.room = roomservice.GetRoomFromID(reservation.RoomID);
                var receiptCard = new HeroCard
                {
                    Title = room.Name.ToString(),
                    Text = "Ngày đến: " +  reservation.CheckInDateTime.Date.ToString("dd/MM/yyyy") +
                    "\n\nNgày đi: " +  reservation.CheckOutDateTime.Date.ToString("dd/MM/yyyy") 
                };
                Activity activity = context.Activity as Activity;
                IMessageActivity messageCard = activity.CreateReply();
                messageCard.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                messageCard.Attachments.Add(receiptCard.ToAttachment());
                await context.PostAsync(messageCard);

            }
            PromptDialog.Confirm(context, ConfirmedRoomReservation, "Thông tin này đã đúng chưa ạ?");
        }
        public async Task ConfirmedRoomReservation(IDialogContext context, IAwaitable<bool> argument)
        {
            bool isCorrect = await argument;
            if (isCorrect)
            {
                var customerDialog = new FormDialog<Customer>(this.customer, Customer.BuildCustomerForm, FormOptions.PromptInStart);
                context.Call(customerDialog, this.ResumeAfterCustomerDialog);
            }
            else
            {
                var reservationDialog = new FormDialog<Reservation>(this.reservation, Reservation.BuildOrderForm, FormOptions.PromptInStart);
                context.Call(reservationDialog, this.ResumeAfterReservationDialog);
            }
        }

        private async Task ResumeAfterCustomerDialog(IDialogContext context, IAwaitable<Customer> result)
        {
            var customer = await result;
            if(customer is Customer)
            {
                this.customer = customer;
            }
            this.reservation.CustomerID = this.customer.ID;
            using (RoomService roomservice = new RoomService())
            {
                Room room = roomservice.GetRoomFromID(reservation.RoomID);
                var receiptCard = new ReceiptCard
                {
                    Title = this.customer.Name,
                    Items = new List<ReceiptItem> {
                    new ReceiptItem
                    {
                         Title = room.Name,
                         Image = new CardImage(HostValueUtils.DOMAIN + room.Image),
                    } },
                    Facts = new List<Fact> {
                        new Fact("Ngày đến:", reservation.CheckInDateTime.ToString("dd/MM/yyyy").ToString()),
                        new Fact("Ngày đi:", reservation.CheckOutDateTime.ToString("dd/MM/yyyy").ToString()),
                        new Fact("Số điện thoại:", customer.Phone.ToString()),
                        new Fact("CMND:", customer.PassportNumber.ToString())
                    },
                    Total = (room.Price* (((reservation.CheckOutDateTime - reservation.CheckInDateTime).Days))).ToString()
                };
                Activity activity = context.Activity as Activity;
                IMessageActivity message = activity.CreateReply();
                message.Attachments.Add(receiptCard.ToAttachment());
                await context.PostAsync(message);
            }

            PromptDialog.Confirm(context, ConfirmedReservation, "Quý khách có muốn xác nhận Phiếu đặt phòng này không?");
        }

        public async Task ConfirmedReservation(IDialogContext context, IAwaitable<bool> argument)
        {
            bool isCorrect = await argument;
            if (isCorrect)
            {
                //Bắt đầu đẩy lên CSDL thông tin khách hàng và thông tin phiếu đặt phòng
                using (CustomerService customerService = new CustomerService())
                {
                    CustomerModel model = new CustomerModel();
                    model.GetDataFromCustomer(this.customer);
                    CustomerModel savedCustomer = null;
                    if (customerService.GetByPassportNumber(model.PassportNumber) == null)
                    {
                        savedCustomer = customerService.CreateCustomer(model);
                       
                    }
                    else
                    {
                        savedCustomer = customerService.GetByPassportNumber(model.PassportNumber);
                    }
                    this.reservation.CustomerID = savedCustomer.ID;
                    using (ReservationService reservationService = new ReservationService())
                    {
                        ReservationModel reservationModel = new ReservationModel();
                        reservationModel.GetDataFromReservation(this.reservation);
                        //reservationModel.Customer = model;
                        //reservationModel.Room = this.room;
                        ReservationModel savedreservationModel = reservationService.CreateReservation(reservationModel);
                        await context.PostAsync("Đơn đặt phòng đã được lưu với ID:" + savedreservationModel.ID);
                    }

                }
                   
            }
            else
            {
                var reservationDialog = new FormDialog<Reservation>(this.reservation, Reservation.BuildOrderForm, FormOptions.PromptInStart);
                context.Call(reservationDialog, this.ResumeAfterReservationDialog);
            }
        }

        //Sau khi điền các thông tin cho Reservation, lưu các thông tin cần thiết lại vào this.reservation
        //Gọi RoomCategoryDialog để khách chọn loại phòng.
        private async Task ResumeAfterReservationDialog(IDialogContext context, IAwaitable<Reservation> result)
        {
            var reservation = await result;
            this.reservation = reservation as Reservation;
            // context.Call(new RoomCategoryDialog(), this.ResumeAfterRoomCategoryDialog);

            if (checkDatatime(this.reservation.CheckInDateTime, this.reservation.CheckOutDateTime) == true)
                context.Call(new RoomCategoryDialog(), this.ResumeAfterRoomCategoryDialog);
            else
            {
                await context.PostAsync("Thời gian đặt phòng và trả phòng không phù hợp. Mời quý khách nhập lại. Xin cảm ơn!!!");
                var reservationDialog = new FormDialog<Reservation>(this.reservation, Reservation.BuildOrderForm, FormOptions.PromptInStart);
                context.Call(reservationDialog, this.ResumeAfterReservationDialog);
            }

        }
        //check datatime
        Boolean checkDatatime(DateTime t1, DateTime t2)
        {
            if (DateTime.Compare(t1, t2) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //------------------------------------------------------Being modified Code by Huynh Kien Minh ( 1/5/2018 ) finish at line 270
    }
}

