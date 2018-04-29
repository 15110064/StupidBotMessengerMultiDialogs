using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using MultiDialogsBot.Utils;

namespace StupidBotMessengerMultiDialogs.Services
{
    public class RoomService : IDisposable
    {
        public void Dispose()
        {

        }

        public IMessageActivity GetRooms(IMessageActivity message)
        {
            List<Room> postList;
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETALLROOM));

               postList = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));
                ////postList = postList.GetRange(0, 9);
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                foreach (Room p in postList)
                {
                    List<CardImage> imgList = new List<CardImage>
                    {
                        new CardImage(HostValueUtils.DOMAIN + ":" + HostValueUtils.PORTSSL + p.Image)
                    };
                    var heroCard = new HeroCard
                    {
                        Title = p.Name,
                        Text = p.Description,
                        Images = imgList,
                        //Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Đặt phòng này", value: p.ID) }
                    };
                    message.Attachments.Add(heroCard.ToAttachment());
                }
                return message;
            }
        }

        

        public IMessageActivity GetRoomsByRoomType(IMessageActivity message)
        {
            message.Text = "API này chưa được cung cấp";
                return message;
            
        }

        //public IMessageActivity GetRooms1(IMessageActivity message)
        //{
        //    using (WebClient wc = new WebClient())
        //    {
        //        //                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
        //        //                var json = (wc.DownloadString(HostValueUtils.GETALLROOM));
        //        //List<Room> postList = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));
        //        ////postList = postList.GetRange(0, 9);
        //        message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //        //                foreach (Room p in postList)
        //        //                {
        //        for (int i = 0; i < 5; i++)
        //        {
        //            List<CardImage> imgList = new List<CardImage>
        //            {
        //                new CardImage(@"https://i.imgur.com/vAHUOP5.png")
        //            };
        //            var heroCard = new HeroCard
        //            {
        //                Title = "nachos",
        //                Text = "nachos",
        //                Images = imgList,
        //                Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "stuff", value: "things") }
        //            };
        //            message.Attachments.Add(heroCard.ToAttachment());
        //        }

        //        //                }
        //        return message;
        //    }
        //}

        public Room JsonToRoom(Newtonsoft.Json.Linq.JObject jsonOject)
        {
            Room room = (Room)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonOject.ToString(), typeof(Room));
            return room;
        }
    }

}


 