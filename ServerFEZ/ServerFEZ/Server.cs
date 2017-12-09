using System;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace ServerFEZ
{
    class Server
    {
        public static void Main(string[] args)
        {
            new Server();
        }

        public Server()
        {
            localEndPoint = new IPEndPoint(IPAddress.Parse(Constants.IP_LOCAL), Constants.PORT_TCP);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            startServer();
        }

        private void startServer()
        {
            listener.Bind(localEndPoint);
            listener.Listen(1);
            while (true)
            {
                Console.WriteLine("Waiting for picture...");
                Socket handler = listener.Accept();
                Console.WriteLine("FEZ CONNECTED");
                Console.WriteLine("handler " + handler.Blocking);
                handler.Blocking = true;
                receiveFromFez(handler);
            }
        }

        private void receiveFromFez(Socket handler)
        {
            //TODO change



            byte[] pictureData;
            int pictureSize = 0, received = 0, sent = 0; ;
            SocketError error;
            try
            {
                handler.ReceiveTimeout = 0;
                handler.SendTimeout = 0;

                byte[] pictureByteSize = new byte[sizeof(int)];

                received = handler.Receive(pictureByteSize, 0, sizeof(int), SocketFlags.None, out error);
                if (error == SocketError.Shutdown || error == SocketError.ConnectionReset)
                {
                    Console.WriteLine("errore " + error.ToString());
                    handler.Close();
                    return;
                }
                pictureSize = BitConverter.ToInt32(pictureByteSize, 0);
                Console.WriteLine("PICTURE SIZE {0} ", pictureSize);
                pictureData = new byte[pictureSize];
                received = 0;

                while (received < pictureSize)
                {
                    if (pictureSize - received > Constants.PACKET_SIZE)
                        received += handler.Receive(pictureData, received, Constants.PACKET_SIZE, SocketFlags.None, out error);
                    else received += handler.Receive(pictureData, received, pictureSize - received, SocketFlags.None, out error);
                    if (error == SocketError.Shutdown || error == SocketError.ConnectionReset)
                    {
                        Console.WriteLine("errore " + error.ToString());
                        handler.Close();
                        return;
                    }
                }
            }
            catch (SocketException s)
            {
                handler.Close();
                Console.WriteLine("SOCKET CLOSED EXCEPTION");
                Console.WriteLine("errore " + s.Message);
                return;
            }
            Console.WriteLine("PictureSize: {0},received {1} ", pictureSize, received);

            Bitmap bitmap;
            using (var ms = new MemoryStream(pictureData))
            {
                bitmap = new Bitmap(ms);
            }

            byte[] responseToFEZ = new byte[Constants.EVALUATION.ACCEPT.ToString().Length];
            var barcodeReader = new BarcodeReader();
            //var barcodeBitmap = (Bitmap)Bitmap.FromFile(@"C:\Users\Cristiano\Desktop\jpeg.jpg");
            var barcodeResult = barcodeReader.Decode(bitmap);
            if (barcodeResult != null)
                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.ACCEPT.ToString());
            else
                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());


            //Console.WriteLine($"Decoded barcode text: {barcodeResult?.Text}");
            //Console.WriteLine($"Barcode format: {barcodeResult?.BarcodeFormat}");
            //responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.REJECT.ToString());
            //responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());


            //CONTACT WEBSERVER
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = client.GetAsync("http://192.168.1.163:8082/RestfulService/resources/auth/1/3").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    string str = response.Content.ReadAsStringAsync().Result;
            //    Console.WriteLine("str {0}", str);
            //}

            try
            {
                sent = handler.Send(responseToFEZ, 0, responseToFEZ.Length, SocketFlags.None, out error);
                if (error == SocketError.Shutdown || error == SocketError.ConnectionReset)
                {
                    Console.WriteLine("errore " + error.ToString());
                    handler.Close();
                    return;
                }
            }
            catch (SocketException e)
            {
                handler.Close();
                return;
            }
            handler.Close();
            Image x = (Bitmap)((new ImageConverter()).ConvertFrom(pictureData));
            x.Save(@"C:\Users\Cristiano\Desktop\image.jpeg", ImageFormat.Jpeg);
        }

        private IPEndPoint localEndPoint = null;
        private Socket listener = null;
    }
}