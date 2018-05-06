using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Model
{
    public class ReservationModel
    {
        public void GetDataFromReservation(Reservation reservation)
        {
            this.ID = reservation.ID;
            this.CheckInDateTime = reservation.CheckInDateTime;
            this.CheckOutDateTime = reservation.CheckOutDateTime;
            this.Note = reservation.Note;
            this.Quantity = reservation.Quantity;
            this.Price = reservation.Price;
            this.CustomerID = reservation.CustomerID;
            this.RoomID = reservation.RoomID;
            this.Total = reservation.Total;
            this.Deposit = reservation.Deposit;
            this.IsPaid = reservation.IsPaid;
            this.Room = null;
            this.Customer = null;
            this.CreatedDate = null;
            this.CreatedBy = null;
            this.UpdatedBy = null;
            this.MetaDescription = null;
            this.MetaKeyword = null;
            this.Status = false;
        
    }
        public int ID { set; get; }

        public DateTime CheckInDateTime { get; set; }

        public DateTime CheckOutDateTime { get; set; }

        public string Note { get; set; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int CustomerID { set; get; }

        public int RoomID { get; set; }

        public decimal Total { get; set; }

        public decimal? Deposit { get; set; }

        public bool IsPaid { get; set; }

        public virtual Room Room { set; get; }

        public virtual CustomerModel Customer { set; get; }
        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }
    }
}