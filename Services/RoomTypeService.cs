using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Services
{
    public class RoomTypeService : IDisposable
    {
        public void Dispose()
        {

        }

        public IMessageActivity GetRoomTypes(IMessageActivity message)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString("http://localhost:51607/api/roomtype/getall?keyword=&page=0&pageSize=25"));

                RootObject items = (RootObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(RootObject));

                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                foreach (RoomType p in items.Items)
                {
                    var heroCard = new ThumbnailCard
                    {
                        Title = p.Name,
                        Subtitle = p.Description,
                    };

                    message.Attachments.Add(heroCard.ToAttachment());
                }
                return message;
            }
        }
    }
}