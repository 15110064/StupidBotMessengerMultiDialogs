using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Asserts;
using StupidBotMessengerMultiDialogs.Model;
using StupidBotMessengerMultiDialogs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Dialogs
{
    [Serializable]
    public class RoomCategoryDialog : PagedCarouselDialog<object>
    {
        public Reservation Reservation { get; set; }
        public List<Room> Rooms { get; set; } =
        new List<Room>();
        HashSet<int> ls = new HashSet<int>();
        public int CurrentRoomTypeId { get; set; }
        //public RoomType CurentRoomType { get; set; }
        public int Capacity { get; set; }
        public RoomCategoryDialog()
        {
        }
        public RoomCategoryDialog(Reservation reservation)
        {
            this.Reservation = reservation;
            //this.Rooms = new List<Room>();
            this.Capacity = 0;
        }

        public override string Prompt
        {
            get { return "Vui lòng chọn loại phòng:"; }
        }

        //Hàm này gọi service để truy vấn dữ liệu
        public override PagedCarouselCards GetCarouselCards(int pageNumber, int pageSize)
        {
           List<HeroCard> heroCards;
           using (RoomTypeService roomTypeService = new RoomTypeService())
            {
                heroCards = roomTypeService.GetHeroCardsRoomType(Reservation.CheckInDateTime, Reservation.CheckOutDateTime);
            }
            return new PagedCarouselCards
            {
                Cards = heroCards,
                TotalCount = heroCards.Capacity
            };
        }

        public override async Task ProcessMessageReceived(IDialogContext context, string roomCategoryName)
        {
            List<string> roomTypeCatIds;
            using (RoomTypeService roomTypeService = new RoomTypeService())
            {
                roomTypeCatIds = roomTypeService.GetRoomTypeID();
                //CurentRoomType =roomTypeService.GetRoomTypeById()
            }
            if (roomTypeCatIds.IndexOf(roomCategoryName) !=-1)
            {
                CurrentRoomTypeId = int.Parse(roomCategoryName);
                //await this.GetAvailableRoooms(context);
                int numRooms = 0;
                using (RoomService roomService = new RoomService())
                {
                    numRooms = (roomService.GetRoomsAvailable(Reservation.CheckInDateTime.ToString("MM/dd/yyyy"), 
                        Reservation.CheckOutDateTime.ToString("MM/dd/yyyy"), CurrentRoomTypeId)).Count;
                }
                context.Call(new RoomDialog(this.Reservation, CurrentRoomTypeId, numRooms), this.ResumeAfterRoomDialog);
                //context.Done(roomCategoryName);
            }
            else
            {
                //await context.PostAsync(string.Format(CultureInfo.CurrentCulture, Resources.FlowerCategoriesDialog_InvalidOption, flowerCategoryName));
                await this.ShowProducts(context);
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterRoomDialog(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            //this.reservation.RoomID = Convert.ToInt32(message);
            int numRooms = Convert.ToInt32(message);

            RoomService roomService = new RoomService();
            List<Room> roomsAvailable = roomService.GetRoomsAvailable(Reservation.CheckInDateTime.ToString("MM/dd/yyyy"), Reservation.CheckOutDateTime.ToString("MM/dd/yyyy"), CurrentRoomTypeId);

            using (RoomTypeService roomTypeService = new RoomTypeService())
            {
                RoomType roomType = roomTypeService.GetRoomTypeById(CurrentRoomTypeId);
                //Capacity += roomType.MaxPeople * numRooms;
            
            for (int i = 0; i < numRooms; i++)
            {
                    //this.Rooms.Add(roomsAvailable[i]);
                    if (!ls.Contains(roomsAvailable[i].ID))
                    {
                        ls.Add(roomsAvailable[i].ID);
                        Capacity += roomType.MaxPeople;
                    }  
            }
            }
            if (Capacity < Reservation.Adult)
            {
                await context.PostAsync("Số phòng hiện tại không đáp ứng đủ lượng khách. Vui lòng chọn thêm phòng.");
                await this.ShowProducts(context);
                //ShowChooseMoreRoom(context);
            }
            else
            {
                //ls = new List<int>();
                
                
                //foreach (Room r in Rooms)
                //{
                //    ls.Add(r.ID);
                //}
                context.Done(ls.ToList());
            }
            //context.Call(this.dialogFactory.Create<BouquetsDialog, string>(this.order.FlowerCategoryName), this.AfterBouquetSelected);
        }

        private void ShowChooseMoreRoom(IDialogContext context)
        {
            PromptDialog.Confirm(
                                context,
                                OnOptionSelected,
                                "Quý khách có muốn chọn thêm phòng không?",
                                "Vui lòng chọn lại!",
                                promptStyle: PromptStyle.Auto);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<bool> result)
        {
            var confirm = await result;
            if (confirm)
            {
                await this.ShowProducts(context);

                context.Wait(this.MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync("Cảm ơn quý khách!");
                context.Done<object>(result);
            }
        }

       
       private void UpdateCapacity(int roomTypeId, int numRooms)
        {
            using(RoomTypeService roomTypeService = new RoomTypeService())
            {
                RoomType roomType = roomTypeService.GetRoomTypeById(roomTypeId);
                Capacity =  roomType.MaxPeople * numRooms;
            }
        }
        private async Task GetAvailableRoooms(IDialogContext context)
        {
            using (RoomService roomService = new RoomService())
            {
                Rooms = roomService.GetRoomsAvailable(Reservation.CheckInDateTime.ToString("MM/dd/yyyy"), Reservation.CheckOutDateTime.ToString("MM/dd/yyyy"), CurrentRoomTypeId);
            }
        }

    }
}