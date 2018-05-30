namespace StupidBotMessengerMultiDialogs.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Text;
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
        private List<Room> rooms;
        private List<int> ls;
        //private ReservationAndRooms ReservationAndRooms;

        /// <summary>
        /// Ở constructor, khởi tạo 2 biến thành viên là customer và reservation.
        /// </summary>
        public RootDialog()
        {
            customer = new Customer();
            customer.DateOfBirth = new DateTime(1900, 01, 01);
            reservation = new Reservation();
            rooms = new List<Room>();
            //ReservationAndRooms = new ReservationAndRooms();
        }

        public RootDialog(IDialogContext context)
        {
            customer = new Customer();
            reservation = new Reservation();
           rooms = new List<Room>();
        }

        public RootDialog(bool isCallBooking)
        {
            customer = new Customer();
            reservation = new Reservation();
        }

        public async Task StartAsync(IDialogContext context)
        {
            //Root dialog khởi tạo và chờ message tiếp theo từ user
            //Khi một message đến, gọi MessageReceivedAsync.
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
                "Xin chào quý khách! \n\nQuý khách muốn đặt phòng hay tra cứu thông tin?",
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
                        this.reservation = new Reservation();
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
            //if (message.Equals("ReMakeReservation"))
            //{
            //    var reservationDialog = new FormDialog<Reservation>(this.reservation, Reservation.BuildOrderForm, FormOptions.PromptInStart);
            //    context.Call(reservationDialog, this.ResumeAfterReservationDialog);
            //}
            //else if (message.Equals("DontReMakeReservation"))
            //{
            //    await context.PostAsync("Tạm biệt quý khách!");
            //    context.Wait(this.MessageReceivedAsync);
            //}
            //else
            //{
                //this.reservation = (message as ReservationAndRooms).Reservation;

                    //this.reservation = message as Reservation;
                    //this.ReservationAndRooms = message as ReservationAndRooms;
                    this.ls = message as List<int>;
            //Tạo ReceiptCard để hỏi xem thông tin đã đúng chưa
            using (RoomService roomservice = new RoomService())
            {
                //await context.PostAsync("Thông tin phòng:");
                //foreach(int id in ls)
                //{
                //    rooms.Add(roomservice.GetRoomFromID(id));
                //}
                ////this.room = roomservice.GetRoomFromID(reservation.RoomID);
                //StringBuilder stringBuilder = new StringBuilder();

                //foreach (Room room in rooms)
                //{

                //    stringBuilder.Append(room.Name);
                //    stringBuilder.Append("\n\n");
                //}

                var receiptCard = new HeroCard
                    {
                        Title = "Thông tin dự kiến:",
                        Text = "Ngày đến: " + reservation.CheckInDateTime.Date.ToString("dd/MM/yyyy") +
                        "\n\nNgày đi: " + reservation.CheckOutDateTime.Date.ToString("dd/MM/yyyy") +
                        "\n\nSố phòng: " + ls.Count.ToString() + "\n\n" +
                    //stringBuilder.ToString() +
                    "\n\nNgười lớn: " + reservation.Adult.ToString() +
                    "\n\nTrẻ em: " + reservation.Child.ToString()
                };
                    Activity activity = context.Activity as Activity;
                    IMessageActivity messageCard = activity.CreateReply();
                    messageCard.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    messageCard.Attachments.Add(receiptCard.ToAttachment());
                    await context.PostAsync(messageCard);
                   PromptDialog.Confirm(context, ConfirmedRoomReservation, "Thông tin này đã đúng chưa ạ?");
                //}

           }
            //PromptDialog.Confirm(context, ConfirmedRoomReservation, "Thông tin này đã đúng chưa ạ?");
        }

        private void ShowOptionsConfirm(IDialogContext context)
        {
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
                //Room room = roomservice.GetRoomFromID(reservation.RoomID);
                decimal totals = roomservice.GetPrice(ls);
                this.reservation.Total = totals * Convert.ToDecimal((reservation.CheckOutDateTime - reservation.CheckInDateTime).TotalDays);
                this.reservation.Price = totals * Convert.ToDecimal((reservation.CheckOutDateTime - reservation.CheckInDateTime).TotalDays);
                //var receiptCard = new ReceiptCard
                //{
                //    Title = this.customer.Name,
                //    //Items = new List<ReceiptItem> {
                //    //new ReceiptItem
                //    //{
                //    //     Title = room.Name,
                //    //     Image = new CardImage(HostValueUtils.DOMAIN + room.Image),
                //    //} },
                //    Facts = new List<Fact> {
                //        new Fact("Ngày đến:", reservation.CheckInDateTime.ToString("dd/MM/yyyy").ToString()),
                //        new Fact("Ngày đi:", reservation.CheckOutDateTime.ToString("dd/MM/yyyy").ToString()),
                //        new Fact("Số điện thoại:", customer.Phone.ToString()),
                //        new Fact("CMND:", customer.PassportNumber.ToString())
                //    },
                //    Total = totals.ToString()
                //    //Total = (room.Price* (((reservation.CheckOutDateTime - reservation.CheckInDateTime).Days))).ToString()
                //};
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Khách hàng: " + this.customer.Name + "\n\n");
                stringBuilder.Append("Ngày đến: " + reservation.CheckInDateTime.ToString("dd/MM/yyyy").ToString() + "\n\n");
                stringBuilder.Append("Ngày đi: " + reservation.CheckOutDateTime.ToString("dd/MM/yyyy").ToString() + "\n\n");
                stringBuilder.Append("Số điện thoại: " + customer.Phone.ToString() + "\n\n");
                stringBuilder.Append("CMND: " + customer.PassportNumber.ToString() + "\n\n");
                stringBuilder.Append("Tổng số tiền: " + reservation.Total.ToString() + "\n\n");

                Activity activity = context.Activity as Activity;
                IMessageActivity message = activity.CreateReply();
                message.Text = stringBuilder.ToString();
                //message.Attachments.Add(receiptCard.ToAttachment());
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
                    //CustomerService customerService = new CustomerService();
                    CustomerModel model = new CustomerModel();
                    model.GetDataFromCustomer(this.customer);
                    int savedCustomer = await customerService.GetIDCustomerByPassport(model.PassportNumber);
                    if (savedCustomer == 0)
                    {
                    //customerService = new CustomerService();
                    this.reservation.CustomerID = customerService.CreateCustomerReturnID(model);
                       
                    }
                    else
                    {
                    this.reservation.CustomerID = await customerService.GetIDCustomerByPassport(model.PassportNumber);
                    }
                    //this.reservation.CustomerID = savedCustomer;
                    ReservationModel reservationModel = new ReservationModel();
                    reservationModel.GetDataFromReservation(this.reservation);
                    //using (RoomService roomService = new RoomService())
                    //{
                    //    foreach (int id in ls)
                    //    {
                    //        rooms.Add(roomService.GetRoomFromID(id));
                    //    }
                    //}
                    //this.reservation.Price = (room.Price * (((reservation.CheckOutDateTime - reservation.CheckInDateTime).Days)));
                    using (ReservationService reservationService = new ReservationService())
                    {
                        
                        //reservationModel.Customer = model;
                        //reservationModel.Room = this.room;
                        ReservationAndRooms2 reservationAndRooms = new ReservationAndRooms2
                        {
                            ReservationViewModel = reservationModel,
                            Rooms = ls
                        };
                        ReservationModel savedreservationModel = reservationService.CreateReservationAndRooms2(reservationAndRooms);
                        await context.PostAsync("Đơn đặt phòng đã được lưu. Vui lòng mang theo giấy CMND của quý khách và các hành khách đi cùng đến khách sạn để làm thủ tục nhận phòng.\n\nCảm ơn quý khách đã chọn Khách Sạn Mai Sơn.");
                       // context.Done(true);
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

            //List<HeroCard> heroCards;
            //IMessageActivity message = context.MakeMessage();
            //using (RoomService roomService = new RoomService())
            //{
            //    heroCards = roomService.GetRoomHeroCards(reservation.CheckInDateTime, reservation.CheckOutDateTime, reservation.Child + reservation.Adult)
            //    //heroCards = roomTypeService.GetRooms()
            //};

            //message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            //foreach(HeroCard heroCard in heroCards)
            //{
            //    message.Attachments.Add(heroCard.ToAttachment());
            //}


            //PagedCarouselCards pagedCarouselCards = new PagedCarouselCards
            //{
            //    Cards = heroCards,
            //    TotalCount = heroCards.Capacity
            //};


            //context.Call(new RoomDialog(this.reservation), this.ResumeAfterRoomCategoryDialog);
            context.Call(new RoomCategoryDialog(this.reservation), this.ResumeAfterRoomCategoryDialog);

            //if (checkDatatime(this.reservation.CheckInDateTime, this.reservation.CheckOutDateTime) == true)
            //    context.Call(new RoomCategoryDialog(), this.ResumeAfterRoomCategoryDialog);
            //else
            //{
            //    await context.PostAsync("Thời gian đặt phòng và trả phòng không phù hợp. Mời quý khách nhập lại. Xin cảm ơn!!!");
            //    var reservationDialog = new FormDialog<Reservation>(this.reservation, Reservation.BuildOrderForm, FormOptions.PromptInStart);
            //    context.Call(reservationDialog, this.ResumeAfterReservationDialog);
            //}
        }
      
    }
}

