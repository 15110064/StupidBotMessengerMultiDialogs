using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Model
{
    public class ReservationAndRooms2
    {
        public ReservationAndRooms2()
        {
            Rooms = new List<int>();
        }
        public ReservationModel ReservationViewModel { get; set; }
        public List<int> Rooms { get; set; }
    }
}