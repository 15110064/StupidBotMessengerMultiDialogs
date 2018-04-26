using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiDialogsBot.Utils
{
    public class HostValueUtils
    {
        public static string DOMAIN = "https://sohot-webapp.azurewebsites.net";
        public static string PORTSSL = "443";
        public static string PORT = "80";
        public static string ROOMAPI = "/api/room";
        public static string ROOMTYPEAPI = "/api/roomtype";
        public static string GETALLROOM = DOMAIN + ":"+ PORTSSL+ ROOMAPI + "/getallparents";
        public static string GETALLROOMTYPE = DOMAIN + ":" + PORTSSL + ROOMTYPEAPI + "/getallparents";
    }
}