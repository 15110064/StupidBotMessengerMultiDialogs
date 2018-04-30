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
    public class RoomCategoryDialog : PagedCarouselDialog<string>
    {
        public RoomCategoryDialog()
        {
        }

        public override string Prompt
        {
            get { return "Vui lòng chọn loại phòng:"; }
        }

        public override PagedCarouselCards GetCarouselCards(int pageNumber, int pageSize)
        {

            List<HeroCard> heroCards;
           using (RoomTypeService roomTypeService = new RoomTypeService())
            {
                heroCards = roomTypeService.GetHeroCardsRoomType();
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
            }
            if (roomTypeCatIds.IndexOf(roomCategoryName) !=-1)
            {
                context.Call(new RoomDialog(), this.ResumeAfterRoomDialog);
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
            //await context.PostAsync($"Nhan duoc phong: "+ message);
            context.Done(message);
            //context.Call(this.dialogFactory.Create<BouquetsDialog, string>(this.order.FlowerCategoryName), this.AfterBouquetSelected);
        }
    }
}