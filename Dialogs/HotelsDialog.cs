namespace StupidBotMessengerMultiDialogs.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;
    using StupidBotMessengerMultiDialogs.Model;
    using StupidBotMessengerMultiDialogs.Services;

    [Serializable]
    public class HotelsDialog : IDialog<object>
    {
        private HotelsQuery hotelQuery;
        public async Task StartAsync(IDialogContext context)
        {
            hotelQuery = new HotelsQuery();
            if(context.Activity is Activity)
            {
                using (RoomService service = new RoomService())
                    hotelQuery.MãPhòng = service.JsonToRoom((context.Activity as Activity).Value as Newtonsoft.Json.Linq.JObject).Name;
                await context.PostAsync(hotelQuery.MãPhòng);

            }

            await context.PostAsync("Cảm ơn quý khách đã chọn Khách sạn Mai Sơn!");
            var hotelsFormDialog = FormDialog.FromForm(this.BuildHotelsForm, FormOptions.PromptInStart);
            context.Call(hotelsFormDialog, this.ResumeAfterHotelsFormDialog);
        }

        private IForm<HotelsQuery> BuildHotelsForm()
        {
            OnCompletionAsyncDelegate<HotelsQuery> processHotelsSearch = async (context, state) =>
            {
                string message = $"Ghi nhận phiếu đặt phòng:\n\n" +
                    $"**Phòng:** {state.MãPhòng}\n\n" +
                    $"**Thời gian check in:** {state.NhậnPhòng.ToString("MM/dd : HH:mm ")}\n\n" +
                    $"**Tên:** {state.Name}\n\n" +
                    $"**Số CMND:** {state.PassportNumber}\n\n" +
                    $"**Số điện thoại:** {state.PhoneNumber}";
                await context.PostAsync(message);
            };
           
            return new FormBuilder<HotelsQuery>()
                .Field(nameof(HotelsQuery.Name))
                //.Message("Looking for hotels in {LoạiPhòng}...")
                .AddRemainingFields()
                .OnCompletion(processHotelsSearch)
                .Build();
        }

        private async Task ResumeAfterHotelsFormDialog(IDialogContext context, IAwaitable<HotelsQuery> result)
        {
          
            context.Done<object>(null);
        }

        private async Task<IEnumerable<HotelRoom>> GetHotelsAsync(HotelsQuery searchQuery)
        {
            var hotels = new List<HotelRoom>();

            // Filling the hotels results manually just for demo purposes
            for (int i = 1; i <= 5; i++)
            {
                var random = new Random(i);
                HotelRoom hotelRoom = new HotelRoom()
                {
                    Name = $"{searchQuery.MãPhòng} Hotel {i}",
                    Location = searchQuery.Name,
                    Rating = random.Next(1, 5),
                    NumberOfReviews = random.Next(0, 5000),
                    PriceStarting = random.Next(80, 450),
                    Image = $"https://placeholdit.imgix.net/~text?txtsize=35&txt=Hotel+{i}&w=500&h=260"
                };

                hotels.Add(hotelRoom);
            }

            hotels.Sort((h1, h2) => h1.PriceStarting.CompareTo(h2.PriceStarting));

            return hotels;
        }


        async Task FirstMessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> message)
        {
            var msg = message;
          
        }

    }

    
}