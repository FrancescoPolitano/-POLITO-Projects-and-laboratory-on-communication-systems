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
        public static List<User> GetAllUsers()
        {
            string json = String.Empty;
            //TODO why this and not await?
            HttpResponseMessage response = client.GetAsync(myRest + "/resources/users").Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }


        public static List<Room> GetAllRooms()
        {
            string json = String.Empty;
            //TODO why this and not await? + CHANGE LOCALS WITH ROOMS
            HttpResponseMessage response = client.GetAsync(myRest + "/resources/locals").Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<Room>>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }


        //method to create a new user
        public static EmployeeResponseClass CreateUser(User u)
        {
            EmployeeRequestClass eRC = new EmployeeRequestClass();
            eRC.User = u;
            eRC.Photo = File.ReadAllBytes(u.PathPhoto);

            string json = JsonConvert.SerializeObject(eRC);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(myRest + "/users", content).Result;
            //TODO change the return parameter
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<EmployeeResponseClass>(response.Content.ReadAsStringAsync().Result);
            return null;
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
            HttpResponseMessage response = await client.GetAsync(myRest + "/resources/users/" + id);
            if (response.IsSuccessStatusCode)
                json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(json);
        }

        //method to block accesses from user 
        public static async Task<bool> BlockAccess(User u)
        {
            string json = JsonConvert.SerializeObject(u);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(myRest + "/resources/users/block", content);
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
        public static  bool QRCodeChange(string id)
        {
          
            var content = new StringContent(id, Encoding.UTF8, "application/json");
            HttpResponseMessage response =  client.PostAsync(myRest + "/resources/users/new_code", content).Result;
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }


        public static async Task<bool> Login(string username, string password)
        {

            HttpResponseMessage response = client.GetAsync(myRest + "/login" + "/username=" + username + "password=" + password).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                if (String.Compare(result, "OK") == 0)
                    return true;
            }
            return false;
        }


        public static List<Access> GetHistory(ComplexQuery q)
        {
            string json = JsonConvert.SerializeObject(q);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(myRest + "/resources/query", content).Result;
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<Access>>(response.Content.ReadAsStringAsync().Result);
            return null;
        }









    }
}
