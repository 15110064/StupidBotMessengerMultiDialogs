using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Model
{
    public class RoomType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public int MaxPeople { get; set; }
        public object Image { get; set; }
        public List<object> Rooms { get; set; }
        public DateTime CreatedDate { get; set; }
        public object CreatedBy { get; set; }
        public object UpdatedDate { get; set; }
        public object UpdatedBy { get; set; }
        public decimal Price { set; get; }
        public object MetaKeyword { get; set; }
        public object MetaDescription { get; set; }
        public bool Status { get; set; }
    }

    public class RootObject
    {
        public int Page { get; set; }
        public int Count { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int MaxPage { get; set; }
        public List<RoomType> Items { get; set; }
    }
}