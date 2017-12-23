using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

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

        private static string myRest = Constants.myRest;
        private static string sessionToken = String.Empty;
        private static readonly HttpClient client = new HttpClient();

        //method for getting all users
        public static List<Employee> GetAllUsers()
        {
            string json = String.Empty;
            //TODO why this and not await?
            HttpResponseMessage response = client.GetAsync(myRest + "/users/employees").Result;

            if (response.IsSuccessStatusCode)
            {
                List<Employee> lista =  JsonConvert.DeserializeObject<List<Employee>>(response.Content.ReadAsStringAsync().Result);
                return lista;
            }
            return null;
        }

        //method for getting all visitors
        public static List<Visitor> GetAllVisitors()
        {
            string json = String.Empty;
            //TODO why this and not await?
            HttpResponseMessage response = client.GetAsync(myRest + "/users/visitors").Result;

            if (response.IsSuccessStatusCode)
            {
                List<Visitor> lista = JsonConvert.DeserializeObject<List<Visitor>>(response.Content.ReadAsStringAsync().Result);
                return lista;
            }
            return null;
        }


        public static List<Room> GetAllRooms()
        {
            string json = String.Empty;
            //TODO why this and not await? + CHANGE LOCALS WITH ROOMS
            HttpResponseMessage response = client.GetAsync(myRest + "/locals").Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<Room>>(response.Content.ReadAsStringAsync().Result);
            }
            return null;
        }


        //method to create a new user
        public static EmployeeResponseClass CreateUser(Employee u)
        {
            EmployeeRequestClass eRC = new EmployeeRequestClass
            {
                Employee = u,
                Photo = File.ReadAllBytes(u.PathPhoto)
            };
            AuthenticatedRequest authReq = new AuthenticatedRequest(sessionToken, eRC);
            string json = JsonConvert.SerializeObject(authReq);
            //Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(myRest + "/users/employees", content).Result;
            //TODO change the return parameter
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<EmployeeResponseClass>(response.Content.ReadAsStringAsync().Result);
            return null;
        }

        //method to create a visitor
        //TODO test this
        public static async Task<string> CreateVisitor(Visitor v)
        {
            AuthenticatedRequest authReq = new AuthenticatedRequest(sessionToken, v);
            string json = JsonConvert.SerializeObject(authReq);
            //Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(myRest + "/users/visitors", content);
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            else
                return String.Empty;
        }

        //TODO method to GET a list of users from the service
        public static async Task<Employee> GetSingleUser(string id)
        {
            //only works for 1 guy, how to get more than one?
            string json = String.Empty;
            HttpResponseMessage response = await client.GetAsync(myRest + "/users/" + id);
            if (response.IsSuccessStatusCode)
                json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Employee>(json);
        }

        //method to block accesses from user 
        public static async Task<bool> BlockAccess(Employee u)
        {
            AuthenticatedRequest authReq = new AuthenticatedRequest(sessionToken, u);

            string json = JsonConvert.SerializeObject(authReq);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(myRest + "/users/block", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        //TODO chiedere la query corretta e il parametro da passare
        //method to ask for a change of role
        public static async Task<bool> RoleChange(Employee u)
        {
            AuthenticatedRequest authReq = new AuthenticatedRequest(sessionToken, u);
            string json = JsonConvert.SerializeObject(authReq);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(myRest + "/users/changeRole", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        //method to ask for a change in QRcode
        public static string QRCodeChange(string id)
        {
            AuthenticatedRequest authReq = new AuthenticatedRequest(sessionToken, id);
            string json = JsonConvert.SerializeObject(authReq);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(myRest + "/users/new_code", content).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            else
                return String.Empty;
        }


        public static async Task<bool> Login(LoginData logindata)
        {
            string json = JsonConvert.SerializeObject(logindata);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(myRest + "/login" ,content).Result;
            if (response.IsSuccessStatusCode)
            {
                sessionToken = response.Content.ReadAsStringAsync().Result;
                if (String.IsNullOrEmpty(sessionToken))
                    return false;
                else
                    return true;
            }
            return false;
        }

       


        public static List<Access> GetHistory(ComplexQuery q)
        {
            AuthenticatedRequest authReq = new AuthenticatedRequest(sessionToken, q);
            string json = JsonConvert.SerializeObject(authReq);
            Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(myRest + "/query", content).Result;
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<Access>>(response.Content.ReadAsStringAsync().Result);
            return null;
        }


        public static void Logout()
        {
            AuthenticatedRequest authReq = new AuthenticatedRequest(sessionToken, null);
            string json = JsonConvert.SerializeObject(authReq);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(myRest + "/logout", content).Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception();
            else
                sessionToken = String.Empty;
        }
    }
}
