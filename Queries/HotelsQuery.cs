namespace StupidBotMessengerMultiDialogs
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;
    

    [Serializable]
    public class HotelsQuery
    {
        [Prompt("Quý khách vui lòng nhập {&}")]
        public string MãPhòng { get; set; }
        
        [Prompt("Thời gian {&}?")]
        public DateTime NhậnPhòng { get; set; }

        [Prompt("Họ tên: ")]
        public string Name { get; set; }

        [Prompt("Số CMND: ")]
        public string PassportNumber { get; set; }

        [Prompt("Số điện thoại: ")]
        public string PhoneNumber { get; set; }
    }
}