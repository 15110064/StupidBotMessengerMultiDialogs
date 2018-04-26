using Microsoft.Bot.Connector;
using MultiDialogsBot.Utils;
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
                var json = (wc.DownloadString(HostValueUtils.GETALLROOMTYPE));

                List<RoomType> items = (List<RoomType>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<RoomType>));

                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                foreach (RoomType p in items)
                {
                    var heroCard = new HeroCard
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