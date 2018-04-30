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
                    List<CardImage> imgList = new List<CardImage>
                    {
                        new CardImage(HostValueUtils.DOMAIN + ":" + HostValueUtils.PORTSSL + p.Image)
                    };
                    var heroCard = new HeroCard
                    {
                        Title = p.Name,
                        Subtitle = p.Description,
                        Images = imgList,
                        //Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Xem phòng trống", value: p.ID.ToString()) }
                    };

                    message.Attachments.Add(heroCard.ToAttachment());
                }
                return message;
            }
        }

        public List<HeroCard> GetHeroCardsRoomType()
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETALLROOMTYPE));

                List<RoomType> items = (List<RoomType>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<RoomType>));

                List<HeroCard> heroCards = new List<HeroCard>();
                foreach (RoomType p in items)
                {
                    List<CardImage> imgList = new List<CardImage>
                    {
                        new CardImage(HostValueUtils.DOMAIN + ":" + HostValueUtils.PORTSSL + p.Image)
                    };
                    var heroCard = new HeroCard
                    {
                        Title = p.Name,
                        Subtitle = p.Description,
                        Images = imgList,
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Xem phòng trống", value: p.ID.ToString()) }
                    };

                    heroCards.Add(heroCard);
                }
                return heroCards;
            }
        }

        public List<string> GetRoomTypeID()
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETALLROOMTYPE));

                List<RoomType> items = (List<RoomType>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<RoomType>));

                List<string> roomTypeID = new List<string>();
                foreach (RoomType p in items)
                {
   
                    roomTypeID.Add(p.ID.ToString());
                }
                return roomTypeID;
            }
        }


        public RoomType JsonToRoomType(Newtonsoft.Json.Linq.JObject jsonOject)
        {
            RoomType roomType = (RoomType)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonOject.ToString(), typeof(RoomType));
            return roomType;
        }
    }
}