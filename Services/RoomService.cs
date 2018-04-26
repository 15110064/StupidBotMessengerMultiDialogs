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
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETALLROOM));

                List<Room> postList = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));
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
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Đặt phòng này", value: p) }
                    };
                    message.Attachments.Add(heroCard.ToAttachment());
                }
                return message;
            }
        }

        public Room JsonToRoom(Newtonsoft.Json.Linq.JObject jsonOject)
        {
            Room room = (Room)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonOject.ToString(), typeof(Room));
            return room;
        }
    }
}
 