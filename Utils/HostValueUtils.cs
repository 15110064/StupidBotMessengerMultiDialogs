using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiDialogsBot.Utils
{
    public class HostValueUtils
    {
        public static string DOMAIN = "https://khachsanmaisonweb.azurewebsites.net";
        //public static string DOMAIN = "http://localhost";
        public static string PORTSSL = "443";
        //public static string PORTSSL = "51607";
        public static string PORT = "80";
        public static string ROOMAPI = "/api/room";
        public static string RESERVATIONAPI = "/api/reservation";
        public static string CUSTAPI = "/api/customer";
        public static string ROOMTYPEAPI = "/api/roomtype";
        public static string GETALLROOM = DOMAIN + ":"+ PORTSSL+ ROOMAPI + "/getallparents";
        public static string GETTOTALPRICE = DOMAIN + ":"+ PORTSSL+ ROOMAPI + "/getPrice";
        public static string GETAVAILABLEROOM = DOMAIN + ":" + PORTSSL + ROOMAPI + "/getavailble";
        public static string GETALLROOMTYPE = DOMAIN + ":" + PORTSSL + ROOMTYPEAPI + "/getallparents";
        public static string GETROOMTYPEBYID = DOMAIN + ":" + PORTSSL + ROOMTYPEAPI + "/getbyid/";
        public static string GETALLROOMTYPE_WITH_ROOM_QUANITY = DOMAIN + ":" + PORTSSL + ROOMTYPEAPI + "/getRoomTypeWithRoomQuanity";
        public static string CREATECUSTOMER = DOMAIN + ":" + PORTSSL + CUSTAPI + "/create";
        public static string CREATERESERVATION = DOMAIN + ":" + PORTSSL + RESERVATIONAPI + "/create";
        public static string CREATERESERVATIONANDROOM = DOMAIN + ":" + PORTSSL + RESERVATIONAPI + "/create";
        public static string CREATERESERVATIONANDROOM2 = DOMAIN + ":" + PORTSSL + RESERVATIONAPI + "/create2";
        public static string GETCUSTOMERBYID = DOMAIN + ":" + PORTSSL + CUSTAPI + "/getbyid/";
        public static string GETCUSTOMERBYPHONE = DOMAIN + ":" + PORTSSL + CUSTAPI + "/getbyphone/";
        public static string GETCUSTOMERBYPASSPORT = DOMAIN + ":" + PORTSSL + CUSTAPI + "/getbypassport/";
    }
}