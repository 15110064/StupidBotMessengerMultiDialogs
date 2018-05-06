using MultiDialogsBot.Utils;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace StupidBotMessengerMultiDialogs.Services
{
    public class CustomerService : IDisposable
    {
        public void Dispose()
        {
           
        }
        public CustomerModel CreateCustomer(CustomerModel customer)
        {
            using (WebClient wc = new WebClient())
            {
                string customerJson = Newtonsoft.Json.JsonConvert.SerializeObject(customer, new Newtonsoft.Json.JsonSerializerSettings{ NullValueHandling = Newtonsoft.Json.NullValueHandling.Include});
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string json = wc.UploadString(HostValueUtils.CREATECUSTOMER, customerJson);
                CustomerModel savedCustomer = (CustomerModel)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(CustomerModel));
                return savedCustomer;
            }
        }

        public CustomerModel GetById(int Id)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETCUSTOMERBYID + Id));
                CustomerModel model = (CustomerModel)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(CustomerModel));
                return model;
            }
        }

        public CustomerModel GetByPassportNumber(string passport)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETCUSTOMERBYPASSPORT + passport));
                CustomerModel model = (CustomerModel)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(CustomerModel));
                return model;
            }
        }
    }
}