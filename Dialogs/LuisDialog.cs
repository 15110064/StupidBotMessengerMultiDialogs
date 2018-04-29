using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Enum;
using StupidBotMessengerMultiDialogs.Model;
using StupidBotMessengerMultiDialogs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Dialogs
{
    [LuisModel("61e3e091-1eb4-48ab-ba2c-0d42b7d09052", "33758521eb9944d2aa5e7696c53b81d5")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
      
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Gõ 'help' nếu quý khách cần người hỗ trợ.";

            //alternatively, you could forward to QnA Dialog if no intent is found

            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }


        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            string message = $"Xin chào quý khách.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);

        }

        [LuisIntent("RoomType")]
        public async Task RoomTypeInfo(IDialogContext context, LuisResult result)
        {
            using (RoomTypeService roomTypeService = new RoomTypeService())
            {
                var message = roomTypeService.GetRoomTypes(context.MakeMessage());
                await context.PostAsync(message);
                context.Wait(this.MessageReceived);
                //context.Wait<Activity>(this.RoomTypeClick);
            }
        }

        [LuisIntent("AvailableRoom")]
        public async Task AvailableRoom(IDialogContext context, LuisResult result)
        {

            IMessageActivity messageRooms;
            string message = $"Dưới đây là thông tin các phòng còn trống:";
            await context.PostAsync(message);
            using (RoomService roomService = new RoomService())
            {
                Activity activity = context.Activity as Activity;
                messageRooms = roomService.GetRooms(activity.CreateReply());
                await context.PostAsync(messageRooms);
                context.Wait<Activity>(this.BookRoomClick);

            }

            ////this.ShowOptions(context);
        }

        private async Task BookRoomClick(IDialogContext context, IAwaitable<Activity> result)
        {
            var temp = await result;

            if (temp.Text != null && temp.Text.Length != 0)
            {
                await this.MessageReceived(context, result);
                return;
            }

            //Room receivedRoom;
            //using (RoomService roomService = new RoomService())
            //{
            //    receivedRoom = roomService.JsonToRoom((Newtonsoft.Json.Linq.JObject) temp.Value);
            //}
            
            await context.PostAsync("Nhận được yêu cầu đặt phòng: " + temp.Value);

        }

        private async Task RoomTypeClick(IDialogContext context, IAwaitable<Activity> result)
        {
            var temp = await result;

            if (temp.Text != null && temp.Text.Length != 0)
            {
                await this.MessageReceived(context, result);
                return;
            }

            RoomType receivedRoom;
            using (RoomTypeService roomService = new RoomTypeService())
            {
                receivedRoom = roomService.JsonToRoomType((Newtonsoft.Json.Linq.JObject)temp.Value);
            }
            IMessageActivity messageRooms;
            using (RoomService roomService = new RoomService())
            {
                Activity activity = context.Activity as Activity;
                messageRooms = roomService.GetRoomsByRoomType(activity.CreateReply());
                await context.PostAsync(messageRooms);
                context.Wait<Activity>(this.BookRoomClick);

            }

        }


        [LuisIntent("RoomPrice")]
        public async Task RoomPrice(IDialogContext context, LuisResult result)
        {
            string message = $"Ghi nhận yêu cầu hỏi giá phòng";
            await context.PostAsync(message);


            var options = new Selection[] { Selection.PhongDon, Selection.PhongDoi };
            List<string> BotOptions = new List<string>
            {
                "Phòng đơn",
                "Phòng đôi"
            };
            PromptDialog.Choice(context,
                resume: AfterChooseRoomType,
                options: options,
                prompt: "Vui lòng chọn",
                descriptions: BotOptions,
                promptStyle: PromptStyle.Auto);

        }


        [LuisIntent("BookRoom")]
        public async Task BookRoom(IDialogContext context, LuisResult result)
        {
            string message = $"Ghi nhận yêu cầu đặt phòng";
            await context.PostAsync(message);
           // context.Call(new BookingDialog(), this.ResumeAfterBookingDialog);
        }

        [LuisIntent("Farewell")]
        public async Task Farewell(IDialogContext context, LuisResult result)
        {
            string message = $"Tạm biệt quý khách";
            await context.PostAsync(message);
            context.Done<object>(null);

        }

        [LuisIntent("HotelInformation")]
        public async Task HotelInformation(IDialogContext context, LuisResult result)
        {
            string message = $"Khách sạn Mai Sơn:\n 01 Võ Văn Ngân, Phường Linh Trung, Quận Thủ Đức, Thành phố Hồ Chí Minh\n" +
                $"SĐT: 0827719984 -  Emai: contact@maisonhotel.star";
            await context.PostAsync(message);
            this.ShowOptions(context);
        }


        public async Task AfterChooseRoomType(IDialogContext context, IAwaitable<Selection> argument)
        {

            var confirm = await argument;
            Attachment attachment = null;
            if (confirm == Selection.PhongDon)
            {
                await context.PostAsync("Ghi nhận yêu cầu hỏi giá phòng đơn");
                attachment = new Attachment
                {
                    Name = "BotFrameworkOverview.png",
                    ContentType = "image/png",
                    ContentUrl = "https://3.bp.blogspot.com/-WEXe2xXpF58/WsOQIVlC4lI/AAAAAAAATFw/1vuI-h2ItE0JLwZ20W6MRWOlpodQ_kcFgCLcBGAs/s1600/single.JPG"
                };
            }
            else
            {
                await context.PostAsync("Ghi nhận yêu cầu hỏi giá phòng đôi");
                attachment = new Attachment
                {
                    Name = "BotFrameworkOverview.png",
                    ContentType = "image/png",
                    ContentUrl = "https://4.bp.blogspot.com/-DA7HRHgnVpw/WsORZtLEkTI/AAAAAAAATF8/vEZTOLSPCwAWLrJUzzmWE1d-4kx6ia7lQCLcBGAs/s1600/couple-room.jpg"
                };
            }
            var replyMessage = context.MakeMessage();
            replyMessage.Attachments = new List<Attachment> { attachment };
            await context.PostAsync(replyMessage);

            this.ShowOptions(context);

        }

//        private ResumeAfter<object> After()
//        {
//            //PromptDialog.Confirm(
////                    context,
////                    AfterResetAsync,
////                    "Are you sure you want to reset the count?",
////                    "Didn't get that!",
////                    promptStyle: PromptStyle.Auto);
//        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Confirm(
                                context,
                                OnOptionSelected,
                                "Bạn muốn tra cứu tiếp không?",
                                "Vui lòng chọn lại!",
                                promptStyle: PromptStyle.Auto);
        }


        private async Task OnOptionSelected(IDialogContext context, IAwaitable<bool> result)
        {
            var confirm = await result;
            if (confirm)
            {
                await context.PostAsync("Vui lòng nhập thông tin cần tra cứu");
            }
            else
            {
                await context.PostAsync("Cảm ơn quý khách!");
                context.Done<object>(null);
            }
        }

        private async Task ResumeAfterBookingDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(this.MessageReceived);
        }

    }
}