using MultiDialogsBot.Utils;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Services
{
    public class ReservationService : IDisposable
    {
        public void Dispose()
        {

        }
        public ReservationModel CreateReservation(ReservationModel reservationModel)
        {
            using (WebClient wc = new WebClient())
            {
                string customerJson = Newtonsoft.Json.JsonConvert.SerializeObject(reservationModel, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Include });
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string json = wc.UploadString(HostValueUtils.CREATERESERVATION, customerJson);
                ReservationModel savedReservation = (ReservationModel)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(ReservationModel));
                return savedReservation;
            }
        }

        public ReservationModel CreateReservationAndRooms(ReservationAndRooms reservationAndRooms)
        {
            using (WebClient wc = new WebClient())
            {
                string customerJson = Newtonsoft.Json.JsonConvert.SerializeObject(reservationAndRooms, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Include });
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string json = wc.UploadString(HostValueUtils.CREATERESERVATION, customerJson);
                ReservationModel savedReservation = (ReservationModel)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(ReservationModel));
                return savedReservation;
            }
        }

        public ReservationModel CreateReservationAndRooms2(ReservationAndRooms2 reservationAndRooms)
        {
            using (WebClient wc = new WebClient())
            {
                string customerJson = Newtonsoft.Json.JsonConvert.SerializeObject(reservationAndRooms, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Include });
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string json = wc.UploadString(HostValueUtils.CREATERESERVATIONANDROOM2, customerJson);
                ReservationModel savedReservation = (ReservationModel)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(ReservationModel));
                return savedReservation;
            }
        }
    }
}