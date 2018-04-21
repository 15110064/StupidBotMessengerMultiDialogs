namespace StupidBotMessengerMultiDialogs.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class SupportDialog : IDialog<int>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var ticketNumber = new Random().Next(0, 20000);
           
               

            await context.PostAsync($"Vấn đề '{message.Text}' đã được ghi nhận...");
            string helpStr = $"Dưới đây là một vài mẹo trong khi chờ admin xử lý:\n\n" +
                $"Để yêu cầu phục vụ từ bot, hãy gõ vài từ bất kỳ để bot biết.\n\n" +
               $"Tuy nhiên bạn có thể thực hiện một vài keyword để truy cập nhanh chức năng:\n\n" +
               $"Gõ \"đặt phòng\" để đặt phòng\n\n" +
               $"Gõ \"quit\" để kết thúc một chức năng.\n\n" +
               $"Gõ \"help\" để nhận giúp đỡ.\n\n"
               ;
            await context.PostAsync(helpStr);
            context.Done(ticketNumber);
        }
    }
}
