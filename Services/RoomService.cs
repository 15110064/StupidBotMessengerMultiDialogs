using Microsoft.Bot.Connector;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using MultiDialogsBot.Utils;
using System.Collections.Specialized;

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
                //checkIn=04/06/2018&checkOut=07/06/2018&maxPeople=3
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

        public List<Room> GetRoomsAvailable(string checkIn, string checkOut, int roomTypeId)
        {
            List<Room> postList;
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = wc.DownloadString(HostValueUtils.GETAVAILABLEROOM + "?checkIn=" + checkIn
                    + "&checkOut=" + checkOut
                    + "&maxPeople=" + roomTypeId.ToString());

                postList = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));
                
                return postList;
            }
        }

            public List<HeroCard> GetRoomHeroCards()
        {
            List<Room> postList;
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETALLROOM));

                postList = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));
                List<HeroCard> heroCards = new List<HeroCard>();
                
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
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Đặt phòng này", value: p.ID.ToString()) }
                    };
                    heroCards.Add(heroCard);
                }
                return heroCards;
            }
        }

      

        public List<HeroCard> GetRoomHeroCards(DateTime checkIn, DateTime checkOut, int maxPeople)
        {
            List<Room> postList;
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                //var parameters = new NameValueCollection();
                //wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                //parameters.Add("checkIn", checkIn.ToString("dd/MM/yyyy"));
                //parameters.Add("checkOut", checkOut.ToString("dd/MM/yyyy"));
                //parameters.Add("maxPeople", maxPeople.ToString());

                
                
                //var json = (wc.DownloadString(HostValueUtils.GETAVAILABLEROOM));
                var json = wc.DownloadString(HostValueUtils.GETAVAILABLEROOM + "?checkIn=" + checkIn.ToString("MM/dd/yyyy")
                    + "&checkOut=" + checkOut.ToString("MM/dd/yyyy")
                    + "&maxPeople=" + maxPeople.ToString());

                postList = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));
                List<HeroCard> heroCards = new List<HeroCard>();

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
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Đặt phòng này", value: p.ID.ToString()) }
                    };
                    heroCards.Add(heroCard);
                }
                return heroCards;
            }
        }


        public List<string> GetRoomID()
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETALLROOM));

                List<Room> items = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));

                List<string> roomID = new List<string>();
                foreach (Room p in items)
                {

                    roomID.Add(p.ID.ToString());
                }
                return roomID;
            }
        }

        public Room GetRoomFromID(int Id)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETALLROOM));

                List<Room> items = (List<Room>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Room>));


                return items.Find(p => (p.ID == Id));
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

        public decimal GetPrice(List<int> ids)
        {
            using (WebClient wc = new WebClient())
            {
                string idstring = Newtonsoft.Json.JsonConvert.SerializeObject(ids, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Include });
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string json = wc.UploadString(HostValueUtils.GETTOTALPRICE, idstring);
                decimal savedReservation = (decimal)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(decimal));
                return savedReservation;
            }
        }
    }

}


 