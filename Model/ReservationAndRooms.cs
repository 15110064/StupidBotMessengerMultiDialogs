using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Model
{
    [Serializable]
    public class ReservationAndRooms
    {
        public ReservationAndRooms()
        {
            Rooms = new List<Room>();
        }
        public ReservationModel ReservationViewModel { get; set; }
        public List<Room> Rooms { get; set; }
    }
}