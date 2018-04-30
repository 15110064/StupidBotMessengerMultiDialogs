using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Dialogs
{
    public class CustomerDialog : IDialog<object>
    {
        private Customer customer;
        private Reservation reservation;
        public CustomerDialog(Customer customer, Reservation reservation)
        {
            this.customer = customer;
            this.reservation = reservation;
        }
        public async Task StartAsync(IDialogContext context)
        {
            //var orderForm = new FormDialog<Models.Order>(this.order, Models.Order.BuildOrderForm, FormOptions.PromptInStart);
            //context.Call(orderForm, this.AfterOrderForm);
            var customerDialog = new FormDialog<Customer>(this.customer, Customer.BuildCustomerForm, FormOptions.PromptInStart);
            context.Call(customerDialog, this.AfterCustomerDialog);
        }

        private async Task AfterCustomerDialog(IDialogContext context, IAwaitable<Customer> result)
        {
            var customer = await result;
            context.Done(customer);
        }
    }
}