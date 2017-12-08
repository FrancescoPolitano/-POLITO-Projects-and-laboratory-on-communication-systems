using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    class RestClient
    {
        ////example
        //public static string Get(string uri)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        //    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    using (Stream stream = response.GetResponseStream())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        return reader.ReadToEnd();
        //    }
        //}


        private static readonly HttpClient client = new HttpClient();

        //method for getting all users



        //method to create a new user
        public static async Task<bool> CreateUser(User u)
        {
            string json = JsonConvert.SerializeObject(u);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response =  await client.PostAsync("someuri", content );
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        //method to create a visitor
        public static async Task<bool> CreateVisitor(Visitor v)
        {
            string json = JsonConvert.SerializeObject(v);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("someuri", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        //TODO method to GET a list of users from the service
        public static async Task<User> GetUsers()
        {
            //only works for 1 guy, how to get more than one?
            string json = String.Empty;
            HttpResponseMessage response = await client.GetAsync("someuri");
            if (response.IsSuccessStatusCode)
                json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(json);
        }

        //method to block accesses from user 
        public static async Task<bool> BlockAccess(User u)
        {
            string json = JsonConvert.SerializeObject(u);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("someuri", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        //method to ask for a change of role
        public static async Task<bool> RoleChange(User u)
        {
            string json = JsonConvert.SerializeObject(u);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("someuri", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        //method to ask for a change in QRcode
        public static async Task<bool> QRCodeChange(User u)
        {
            string json = JsonConvert.SerializeObject(u);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("someuri", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        

       

       

       

    }
}
