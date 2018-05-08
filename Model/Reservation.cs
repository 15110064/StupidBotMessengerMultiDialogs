using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Threading;

namespace StupidBotMessengerMultiDialogs.Model
{
    [Serializable]
    
    public class Reservation
    {
       
        public int ID { set; get; }

        [Template(TemplateUsage.NotUnderstood, "Xin lỗi, định dạng ngày bị sai", "Quý khách vui lòng nhập đúng định dạng ngày")]
        [Prompt("Vui lòng nhập vào thời gian nhận phòng (Ví dụ: 25/12/2018):")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime CheckInDateTime { get; set; }
        [Template(TemplateUsage.NotUnderstood, "Xin lỗi, định dạng ngày bị sai", "Quý khách vui lòng nhập đúng định dạng ngày")]
        [Prompt("Vui lòng nhập vào thời gian trả phòng (Ví dụ: 25/12/2018):")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime CheckOutDateTime { get; set; }

        public string Note { get; set; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int CustomerID { set; get; }

        public int RoomID { get; set; }

        public decimal Total { get; set; }

        public decimal? Deposit { get; set; }

        public bool IsPaid { get; set; }

        public static IForm<Reservation> BuildOrderForm()
        {
            return new FormBuilder<Reservation>()
                .Field(nameof(CheckInDateTime),
                      validate: async (state, value) =>
                      {
                          var result = new ValidateResult { IsValid = true, Value = value };
                                                    //If checkoutdate is less than checkin date then its invalid input
                          if (state.CheckInDateTime < DateTime.Now)
                          {
                              result.IsValid = false;
                              result.Feedback = "Ngày đặt phòng phải là hôm nay hoặc ngày mai trở đi";
                          }
                          return result;
                      })
                .Field(nameof(CheckOutDateTime),
                      validate: async (state, value) =>
                      {
                          var result = new ValidateResult { IsValid = true, Value = value };
                          //If checkoutdate is less than checkin date then its invalid input
                          if (state.CheckOutDateTime < DateTime.Now)
                          {
                              result.IsValid = false;
                              result.Feedback = "Ngày trả phòng phải là hôm nay hoặc ngày mai trở đi";
                          }
                          return result;
                      })
                .Build();
        }
    }
}