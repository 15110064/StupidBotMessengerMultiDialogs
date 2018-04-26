using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Model
{
    [Serializable]
    public class Room
    {
        public object RoomType { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string MoreImages { get; set; }
        public decimal Price { get; set; }
        public object Tags { get; set; }
        public bool? HotFlag { get; set; }
        public int RoomTypeID { get; set; }
        public decimal PromotionPrice { get; set; }
        public object ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public object CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public object UpdatedBy { get; set; }
        public object MetaKeyword { get; set; }
        public object MetaDescription { get; set; }
        public bool Status { get; set; }
    }
}