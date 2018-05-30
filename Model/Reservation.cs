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

        [Template(TemplateUsage.NotUnderstood, "Xin lỗi, số người phải là số và lớn hơn hoặc bằng 0", "Quý khách vui lòng nhập đúng số người")]
        [Prompt("Vui lòng nhập số người lớn")]
        public int Adult { get; set; }

        [Template(TemplateUsage.NotUnderstood, "Xin lỗi, số người phải là số và lớn hơn hoặc bằng 0", "Quý khách vui lòng nhập đúng số người")]
        [Prompt("Vui lòng nhập số trẻ em")]
        public int Child { get; set; }

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
                          DateTime checkIn = DateTime.Parse(value.ToString());
                          //If checkoutdate is less than checkin date then its invalid input
                          if (checkIn.Date < DateTime.Now.Date)
                          {
                              result.IsValid = false;
                              result.Feedback = "Ngày đặt phòng phải là hôm nay hoặc ngày mai trở đi";
                          }
                          if((checkIn.Date - DateTime.Now).TotalDays / 365 > 2)
                          {
                              result.IsValid = false;
                              result.Feedback = "Khách sạn không chắc chắn có thể phục vụ được quý khách vào thời điểm trên. Vui lòng kiểm tra lại ngày đặt phòng.";
                          }
                          return result;
                      })
                .Field(nameof(CheckOutDateTime),
                     validate: async (state, value) =>
                     {
                         var result = new ValidateResult { IsValid = true, Value = value };
                         DateTime checkOut = DateTime.Parse(value.ToString());
                         //If checkoutdate is less than checkin date then its invalid input
                         if (checkOut.Date < DateTime.Now.Date)
                         {
                             result.IsValid = false;
                             result.Feedback = "Ngày đặt phòng phải là hôm nay hoặc ngày mai trở đi";
                         }
                         else
                         if (checkOut.Date <= state.CheckInDateTime)
                         {
                             result.IsValid = false;
                             result.Feedback = "Ngày trả phòng phải lớn hơn ngày đặt phòng";
                         }
                         else
                         if ((checkOut.Date - DateTime.Now).TotalDays / 365 > 2)
                         {
                             result.IsValid = false;
                             result.Feedback = "Khách sạn không chắc chắn có thể phục vụ được quý khách vào thời điểm trên. Vui lòng kiểm tra lại ngày trả phòng.";
                         }
                         return result;
                     })
                       .Field(nameof(Adult),
                     validate: async (state, value) =>
                     {
                         var result = new ValidateResult { IsValid = true, Value = value };
                         int adult = Int32.Parse(value.ToString());
                         //If checkoutdate is less than checkin date then its invalid input
                         if (adult <= 0)
                         {
                             result.IsValid = false;
                             result.Feedback = "Số người lớn không thể nhỏ hơn 1";
                         }
                         else if(adult > 20)
                         {
                             result.IsValid = false;
                             result.Feedback = "Hiện tai khách sạn chưa đáp ứng đủ nhu cầu của khách. Vui lòng kiểm tra lại số hành khách đến khách sạn.";
                         }
                         return result;
                     })
                     .Field(nameof(Child),
                     validate: async (state, value) =>
                     {
                         var result = new ValidateResult { IsValid = true, Value = value };
                         int adult = Int32.Parse(value.ToString());
                         //If checkoutdate is less than checkin date then its invalid input
                         if (adult < 0)
                         {
                             result.IsValid = false;
                             result.Feedback = "Số trẻ em không thể nhỏ hơn 0";
                         }else
                         if (adult > 20)
                         {
                             result.IsValid = false;
                             result.Feedback = "Hiện tai khách sạn chưa đáp ứng đủ nhu cầu của khách. Vui lòng kiểm tra lại số hành khách đến khách sạn.";
                         }
                         return result;
                     })

                .Build();
        }
    }
}