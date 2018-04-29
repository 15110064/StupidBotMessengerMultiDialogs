namespace StupidBotMessengerMultiDialogs.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string Booking = "Đặt phòng";

        private const string Asking = "Tra cứu";

        private const string Help = "Giúp đỡ";

       

        public async Task StartAsync(IDialogContext context)
        {
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
            else if(message.Text.ToLower().Contains("đặt phòng"))
            {
                context.Call(new RoomCategoryDialog(), this.ResumeAfterOptionDialog);
            }
            else
            {
                this.ShowOptions(context);
            }
        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { Booking, Asking, Help }, "Bạn muốn đặt phòng hay tra cứu thông tin?", "Vui lòng chọn lại", 3);
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
                        context.Call(new RoomCategoryDialog(), this.ResumeAfterOptionDialog);
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
    }
}
