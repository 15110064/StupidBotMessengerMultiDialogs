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
    public class RoomDialog : PagedCarouselDialog<string>
    {
        
        public RoomDialog()
        {

        }

        public override string Prompt
        {
            get { return "Vui lòng chọn một trong các phòng sau:"; }
        }

        public override PagedCarouselCards GetCarouselCards(int pageNumber, int pageSize)
        {

            List<HeroCard> heroCards;
            using (RoomService roomTypeService = new RoomService())
            {
                heroCards = roomTypeService.GetRoomHeroCards();
            }

            return new PagedCarouselCards
            {
                Cards = heroCards,
                TotalCount = heroCards.Capacity
            };
        }

        public override async Task ProcessMessageReceived(IDialogContext context, string roomId)
        {
            List<string> roomTypeCatIds;
            using (RoomService roomTypeService = new RoomService())
            {
                roomTypeCatIds = roomTypeService.GetRoomID();
            }
            if (roomTypeCatIds.IndexOf(roomId) != -1)
            {
                context.Done(roomId);
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