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
    public class RoomDialog : IDialog<object>
    {
        public Reservation Reservation { get; set; }
        public int RoomType { get; set; }
        public int NumRoomsAvailables { get; set; }

        public RoomDialog()
        {

        }
        public RoomDialog(Reservation reservation, int roomType, int numRoomsAvailables)
        {
            this.Reservation = reservation;
            this.RoomType = roomType;
            this.NumRoomsAvailables = numRoomsAvailables;
        }

        public async Task StartAsync(IDialogContext context)
        {

            var message = context.MakeMessage();
            message.Text = "Vui lòng nhập số lượng phòng cần đặt";
            await context.PostAsync(message);
            context.Wait(this.MessageReceivedAsync);
        }
        //protected async Task GetAvailableRoooms(IDialogContext context)
        //{
        //    var message = context.MakeMessage();
        //    message.Text = "Vui lòng nhập số lượng phòng cần đặt";
        //    await context.PostAsync(message);
        //}

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            
            try
            {
                int numRoom = int.Parse(message.Text);
                if(numRoom < 0)
                {
                    await context.PostAsync("Số lượng phòng không thể nhỏ hơn 0. Vui lòng nhập lại.");
                    context.Wait(this.MessageReceivedAsync);
                }
                else if(numRoom > NumRoomsAvailables)
                {
                    await context.PostAsync("Chỉ còn " + NumRoomsAvailables + " phòng loại này. Vui lòng nhập lại.");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Done(message.Text);
                }
            }
            catch(FormatException ex)
            {
                await context.PostAsync("Vui lòng nhập lại.");
                context.Wait(this.MessageReceivedAsync);
            }

        }

        


    }
}