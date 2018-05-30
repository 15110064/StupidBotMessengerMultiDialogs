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
    [Serializable]
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


        public List<HeroCard> GetHeroCardsRoomType(DateTime checkIn, DateTime checkOut)
        {
            using (WebClient wc = new WebClient())
            {
                //Uri uri = new Uri(string.Format(HostValueUtils.GETALLROOMTYPE_WITH_ROOM_QUANITY + "?checkIn={0}&checkOut={1}", checkIn, checkOut));
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string addressApi = HostValueUtils.GETALLROOMTYPE_WITH_ROOM_QUANITY + "?checkIn="+ checkIn.ToString("MM/dd/yyyy") +"&checkOut="+ 
                    checkOut.ToString("MM/dd/yyyy");
                var json = (wc.DownloadString(addressApi));

                List<RoomType> items = (List<RoomType>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<RoomType>));

                List<HeroCard> heroCards = new List<HeroCard>();
                foreach (RoomType p in items)
                {
                    if (p.Rooms.Count > 0)
                    {
                        List<CardImage> imgList = new List<CardImage>
                    {
                        new CardImage(HostValueUtils.DOMAIN + ":" + HostValueUtils.PORTSSL + p.Image)
                    };
                        var heroCard = new HeroCard
                        {
                            Title = p.Name,
                            Subtitle = p.Description,
                            Text = "Số phòng trống: " + p.Rooms.Count.ToString() + "\n\n Giá: " + p.Price.ToString(),
                            Images = imgList,
                            Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Đặt phòng loại này", value: p.ID.ToString()) }
                        };

                        heroCards.Add(heroCard);
                    }
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

        public RoomType GetRoomTypeById(int id)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETROOMTYPEBYID + id.ToString()));

                RoomType item = (RoomType)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(RoomType));

                
                return item;
            }
        }
    }
}