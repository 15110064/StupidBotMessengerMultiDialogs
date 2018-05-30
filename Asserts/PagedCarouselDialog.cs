using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Model;
using StupidBotMessengerMultiDialogs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Asserts
{
    [Serializable]
    public abstract class PagedCarouselDialog<T> : IDialog<T>
    {
        private int pageNumber = 1;
        private int pageSize = 5;

        public virtual string Prompt { get; }

        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync(this.Prompt ?? "Please select an item");

            await this.ShowProducts(context);

            context.Wait(this.MessageReceivedAsync);
        }

        public abstract PagedCarouselCards GetCarouselCards(int pageNumber, int pageSize);

        public abstract Task ProcessMessageReceived(IDialogContext context, string message);

        protected async Task ShowProducts(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = new List<Attachment>();
            
            //Hàm GetCarouselCards sẽ lấy dữ liệu. Method abstract nên được implement bởi lớp kế thừa

            var productsResult = this.GetCarouselCards(this.pageNumber, this.pageSize);
            //Khi không có phòng phù hợp
            if (productsResult.Cards.Count() <= 0)
            {
                reply.Text = "Xin lỗi, không có phòng nào phù hợp.";
                await context.PostAsync(reply);
                //Gọi một hàm Task ShowMoreOptions truyền vào context
                await this.ShowOptionIfNoRoomFound(context);
            }
            else
            {
                await context.PostAsync(this.Prompt);
                foreach (HeroCard productCard in productsResult.Cards)
                {
                    reply.Attachments.Add(productCard.ToAttachment());
                }
                await context.PostAsync(reply);

                if (productsResult.TotalCount > this.pageNumber * this.pageSize)
                {
                    await this.ShowMoreOptions(context);
                }
            }

            

            //using (RoomTypeService roomTypeService = new RoomTypeService())
            //{
            //    var message = roomTypeService.GetRoomTypes(context.MakeMessage());
            //    await context.PostAsync(message);
            //    //context.Wait(this.MessageReceived);
            //    //context.Wait<Activity>(this.RoomTypeClick);
            //}

           
        }

        protected async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // TODO: validation
            if (message.Text.Equals("Yes, please!", StringComparison.InvariantCultureIgnoreCase))
            {
                this.pageNumber++;
                await this.StartAsync(context);
            }
            else if (message.Text.Equals("Vâng!", StringComparison.InvariantCultureIgnoreCase))
            {
                await this.ProcessMessageReceived(context, "ReMakeReservation");
            }
            else if(message.Text.Equals("Không, cảm ơn!", StringComparison.InvariantCultureIgnoreCase))
            {
                await this.ProcessMessageReceived(context, "DontReMakeReservation");
            }
            else
            {
                await this.ProcessMessageReceived(context, message.Text);
            }
        }

        private async Task ShowMoreOptions(IDialogContext context)
        {
            var moreOptionsReply = context.MakeMessage();
            moreOptionsReply.Attachments = new List<Attachment>
            {
                    new HeroCard()
                {
                    Text = "Want more options?",
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.ImBack, "Yes, please!", value: "Yes, please!")
                    }
                }.ToAttachment()
            };

            await context.PostAsync(moreOptionsReply);
        }

        private async Task ShowOptionIfNoRoomFound(IDialogContext context)
        {
            var moreOptionsReply = context.MakeMessage();
            moreOptionsReply.Attachments = new List<Attachment>
            {
                new HeroCard()
                {
                    Text = "Quý khách muốn chọn ngày khác?",
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.ImBack, "Vâng!", value: "Vâng!"),
                        new CardAction(ActionTypes.ImBack, "Không, cảm ơn!", value: "Không, cảm ơn!")
                    }
                }.ToAttachment(),

            };

            await context.PostAsync(moreOptionsReply);
        }

        public class PagedCarouselCards
        {
            public IEnumerable<HeroCard> Cards { get; set; }

            public int TotalCount { get; set; }
        }

        private async Task Done(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(result);
        }
    }
}