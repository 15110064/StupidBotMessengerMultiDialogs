using MultiDialogsBot.Utils;
using StupidBotMessengerMultiDialogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Bot.Connector;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StupidBotMessengerMultiDialogs.Services
{
    public class CustomerService : IDisposable
    {
        public void Dispose()
        {
           
        }
        public Customer CreateCustomer(CustomerModel customer)
        {
            using (WebClient wc = new WebClient())
            {
                string customerJson = Newtonsoft.Json.JsonConvert.SerializeObject(customer, new Newtonsoft.Json.JsonSerializerSettings{ NullValueHandling = Newtonsoft.Json.NullValueHandling.Include});
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string json = wc.UploadString(HostValueUtils.CREATECUSTOMER, customerJson);
                Customer savedCustomer = (Customer)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(Customer));
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

        public Customer GetByPassportNumber(string passport)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                var json = (wc.DownloadString(HostValueUtils.GETCUSTOMERBYPASSPORT + passport));
                Customer model = (Customer)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(Customer));
                return model;
            }
        }

        public async Task<int> GetIDCustomerByPassport(string passport)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://khachsanmaisonweb.azurewebsites.net:443/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
               // wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json;");
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/customer/getbypassport", passport);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<int>();
                    //var json = (wc.DownloadString(HostValueUtils.GETCUSTOMERBYPASSPORT + passport));
                    //int model = (int)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(int));
                    //return model;
                }
                catch(Exception ex)
                {
                    return 0;
                }
                
                
            }
        }

        //public int GetIDCustomerByPassport(string passport)
        //{
        //    using (WebClient wc = new WebClient())
        //    {
        //        wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
        //        try
        //        {
        //            var json = (wc.DownloadString(HostValueUtils.GETCUSTOMERBYPASSPORT + passport));
        //            int model = (int)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(int));
        //            return model;
        //        }
        //        catch(Exception ex)
        //        {
        //            return 0;
        //        }
                
                
        //    }
        //}

        internal int CreateCustomerReturnID(CustomerModel model)
        {
            using (WebClient wc = new WebClient())
            {
                string customerJson = Newtonsoft.Json.JsonConvert.SerializeObject(model, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Include });
                wc.Headers.Add(header: HttpRequestHeader.ContentType, value: "application/json; charset=utf-8");
                string json = wc.UploadString(HostValueUtils.CREATECUSTOMER, customerJson);
                int savedCustomer = (int)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(int));
                return savedCustomer;
            }
        }
    }
}