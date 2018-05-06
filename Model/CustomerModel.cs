using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Model
{
    public class CustomerModel
    {
        public void GetDataFromCustomer(Customer customer)
        {
            this.ID = customer.ID;
            this.Name = customer.Name;
            this.Phone = customer.Phone;
            this.Email = customer.Email;
            this.Address = customer.Address;
            this.DateOfBirth = customer.DateOfBirth;
            this.Nationality = customer.Nationality;
            this.PassportNumber = customer.PassportNumber;
            this.FacebookID = customer.FacebookID;
            this.CreatedDate = customer.CreatedDate;
            this.CreatedBy = customer.CreatedBy;
            this.UpdatedDate = customer.UpdatedDate;
            this.UpdatedBy = customer.UpdatedBy;
            this.Status = customer.Status;
    }
        public int ID { set; get; }
       
        public string Name { set; get; }
      
        public string Phone { set; get; }
        
        public string Email { set; get; }
       
        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Nationality { get; set; }
       
        public string PassportNumber { get; set; }

        public string FacebookID { get; set; }
        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public bool Status { set; get; }

    }
}