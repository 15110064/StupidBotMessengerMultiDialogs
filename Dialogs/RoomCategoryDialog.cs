using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Asserts;
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
            get { return "Please select an item"; }
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
                await context.PostAsync("Bạn vừa định xem phòng trống cho loại phòng" + roomCategoryName);
                context.Done(roomCategoryName);
            }
            else
            {
                //await context.PostAsync(string.Format(CultureInfo.CurrentCulture, Resources.FlowerCategoriesDialog_InvalidOption, flowerCategoryName));
                await this.ShowProducts(context);
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}