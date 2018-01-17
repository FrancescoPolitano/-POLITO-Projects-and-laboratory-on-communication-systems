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
        public static bool loggedIn = false;
        private static string myRest = Constants.myRest;
        private static CookieContainer cookies = new CookieContainer();
        private static HttpClientHandler handler = new HttpClientHandler()
        {
            CookieContainer = cookies
        };

        private static readonly HttpClient client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        //Login, saves a "cookie"
        public static bool Login(LoginData logindata)
        {
            string json = JsonConvert.SerializeObject(logindata);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(myRest + "/login", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    CookieCollection responseCookies = cookies.GetCookies(new Uri(myRest + "/login"));
                    if (responseCookies["Token"] == null)
                        return false;

                    else
                    {
                        loggedIn = true;
                        return true;
                    }
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
            }
            catch 
            {
                Console.WriteLine("TIMEOUT???");
            }
            return false;
        }

        //logout
        public static void Logout()
        {
            try
            {
                loggedIn = false;
                HttpResponseMessage response = client.GetAsync(myRest + "/logout").Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception();
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();

            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
        }


        //method for getting all users
        public static List<Employee> GetAllUsers()
        {
            string json = String.Empty;
            try
            {
                HttpResponseMessage response = client.GetAsync(myRest + "/users/employees").Result;

                if (response.IsSuccessStatusCode)
                {
                    List<Employee> lista = JsonConvert.DeserializeObject<List<Employee>>(response.Content.ReadAsStringAsync().Result);
                    return lista;
                }
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return GetAllUsers();
                    else
                        return null;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
            return null;
        }

        //method for getting all visitors
        public static List<Visitor> GetAllVisitors()
        {
            string json = String.Empty;
            try
            {


                HttpResponseMessage response = client.GetAsync(myRest + "/users/visitors").Result;

                if (response.IsSuccessStatusCode)
                {
                    List<Visitor> lista = JsonConvert.DeserializeObject<List<Visitor>>(response.Content.ReadAsStringAsync().Result);
                    return lista;
                }
                if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return GetAllVisitors();
                    else
                        return null;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
            return null;
        }

        //method for getting all rooms
        public static List<Room> GetAllRooms()
        {
            string json = String.Empty;
            try
            {
                HttpResponseMessage response = client.GetAsync(myRest + "/locals").Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Room>>(response.Content.ReadAsStringAsync().Result);
                }
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return GetAllRooms();
                    else return null;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
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
            string json = JsonConvert.SerializeObject(eRC);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(myRest + "/users/employees", content).Result;
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<EmployeeResponseClass>(response.Content.ReadAsStringAsync().Result);
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return CreateUser(u);
                    else return null;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
           catch 
            {
                Console.WriteLine("TIMEOUT???");
            }
            return null;
        }

        //method to create a visitor
        public static VisitorResponseClass CreateVisitor(Visitor v)
        {
            string json = JsonConvert.SerializeObject(v);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {

                HttpResponseMessage response = client.PostAsync(myRest + "/users/visitors", content).Result;
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<VisitorResponseClass>(response.Content.ReadAsStringAsync().Result);
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return CreateVisitor(v);
                    else return null;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
            return null;
        }

        //method to get a single user (never used)
        public static async Task<Employee> GetSingleUser(string id)
        {
            string json = String.Empty;
            HttpResponseMessage response = await client.GetAsync(myRest + "/users/" + id);
            if (response.IsSuccessStatusCode)
                json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Employee>(json);
        }

        //method to block accesses from user 
        public static bool BlockAccess(string serial)
        {
            string json = JsonConvert.SerializeObject(serial);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(myRest + "/users/block", content).Result;
                if (response.IsSuccessStatusCode)
                    return true;
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return BlockAccess(serial);
                    else return false;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
            return false;
        }

        //method to ask for a change in QRcode
        public static string QRCodeChange(string id)
        {
            string json = JsonConvert.SerializeObject(id);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(myRest + "/users/new_code", content).Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsStringAsync().Result;
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return QRCodeChange(id);
                    else
                        return String.Empty;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
            return String.Empty;
        }


        //method to get the accesses. 
        public static List<Access> GetHistory(ComplexQuery q)
        {
            string json = JsonConvert.SerializeObject(q);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(myRest + "/query", content).Result;
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<List<Access>>(response.Content.ReadAsStringAsync().Result);
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return GetHistory(q);
                    else return null;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
            return null;
        }

        public static bool ModifyUser(Employee e)
        {
            string json = JsonConvert.SerializeObject(e);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.PostAsync(myRest + "/users/modify", content).Result;
                if (response.IsSuccessStatusCode)
                    return true;
                else if (response.StatusCode == (HttpStatusCode)804)
                {
                    LoginWindow lw = new LoginWindow();
                    lw.ShowDialog();
                    if (loggedIn)
                        return ModifyUser(e);
                    else
                        return false;
                }
            }
            catch (AggregateException tk)
            {
                serviceErrorShow();
                Application.Current.Shutdown();
            }
            catch
            {
                Console.WriteLine("TIMEOUT???");
            }
            return false;

        }
        static private void serviceErrorShow()
        {
            string message = "Cannot connect to the service.";
            string caption = "Connection error";
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBoxResult defaultResult = MessageBoxResult.OK;
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);
        }
    }
}
