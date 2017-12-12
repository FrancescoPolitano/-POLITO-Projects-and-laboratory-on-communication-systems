using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        private static string myRest = "http://192.168.1.171:8082/RestfulService";

        private static readonly HttpClient client = new HttpClient();

        //method for getting all users
        public static async Task<List<User>> GetAllUsers()
        {
            string json = String.Empty;
            //TODO why this and not await?
            HttpResponseMessage response =   client.GetAsync(myRest + "/resources/users").Result;
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                List<User> userList = new List<User>();
                userList = JsonConvert.DeserializeObject<List<User>>(json);
                return userList;
            }
            return null;
        }


        //method to create a new user
        public static async Task<bool> CreateUser(User u)
        {
            string json = JsonConvert.SerializeObject(u);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response =  await client.PostAsync(myRest+"/createUsers", content );
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
            HttpResponseMessage response = await client.PostAsync(myRest + "/createVisitors", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        //TODO method to GET a list of users from the service
        public static async Task<User> GetSingleUser(string id)
        {
            //only works for 1 guy, how to get more than one?
            string json = String.Empty;
            HttpResponseMessage response = await client.GetAsync(myRest + "/resources/users/"+id);
            if (response.IsSuccessStatusCode)
                json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(json);
        }

        //method to block accesses from user 
        public static async Task<bool> BlockAccess(User u)
        {
            string json = JsonConvert.SerializeObject(u);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(myRest+"/resources/users/block", content);
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
            HttpResponseMessage response = await client.PostAsync(myRest + "/resources/users/changeRole", content);
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
            HttpResponseMessage response = await client.PostAsync(myRest + "/resources/users/changeRole", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }


        public static async Task<bool> Login(string username,string password)
        {

            HttpResponseMessage response = client.GetAsync(myRest+"/login" + "/username=" + username + "password=" + password).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                if (String.Compare(result, "OK") == 0)
                    return true;
            }
            return false;
        }


        public static async Task<List<string>> GetHistory(List<string> users, List<string> rooms , string fromDate, string toDate)
        {
            //come cazzo si mette il query parameter in C#
            string query = String.Empty;
            query += "employee=";
            for (int i = 0; i < users.Count; i++)
            {
                query += users[i];
                if (i != (users.Count - 1))
                    query += ",";
            }

            query += "&rooms=";
            for (int i = 0; i < rooms.Count; i++)
            {
                query += rooms[i];
                if (i != (rooms.Count - 1))
                    query += ",";
            }

            if (!String.IsNullOrEmpty(toDate))
                query += "&fromDate=" + toDate;

            if (!String.IsNullOrEmpty(fromDate))
                query += "&toDate=" + fromDate;

            int ii = 0;
            //HttpResponseMessage response =   client.GetAsync(myRest + "?"+ query).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    string pippo = response.Content.ReadAsStringAsync().Result;
            //    List<string> strings = JsonConvert.DeserializeObject<List<String>>(pippo);
            //    return strings;
            //}

            return null;
        }

        

       

       

       

    }
}
