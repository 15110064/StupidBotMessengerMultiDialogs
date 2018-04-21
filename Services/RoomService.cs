using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

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
                var json = wc.DownloadString("https://jsonplaceholder.typicode.com/posts");
                List<Post> postList = (List<Post>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Post>));
                postList = postList.GetRange(0, 9);
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                foreach (Post p in postList)
                {
                    var heroCard = new ThumbnailCard
                    {
                        Title = p.Title,
                        Subtitle = p.Body,

                    };

                    message.Attachments.Add(heroCard.ToAttachment());
                }
                return message;

            }
        }
    }
}