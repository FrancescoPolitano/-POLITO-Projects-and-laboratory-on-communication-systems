using System;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using ZXing;
using System.Text;
using System.IO;
using System.Net.Http;

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
            localEndPoint = new IPEndPoint(IPAddress.Parse(Constants.STATIC_IP_SERVER), Constants.PORT_TCP);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
            startServer();
        }

        private void startServer()
        {
            listener.Bind(localEndPoint);
            listener.Listen(1);
            while (true)
            {
                Console.WriteLine("Waiting for connection...");
                Socket handler = listener.Accept();
                Console.WriteLine("FEZ CONNECTED");
                receiveFromFez(handler);
            }
        }

        private void receiveFromFez(Socket handler)
        {
            while (true)
            {
                byte[] pictureData;
                byte[] responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());
                int pictureSize = 0, received = 0;
                SocketError error;
                Bitmap bitmap = null;
                MemoryStream ms = null;
                try
                {
                    handler.ReceiveTimeout = 8000;
                    handler.SendTimeout = 8000;

                    byte[] pictureByteSize = new byte[sizeof(int)];

                    received = handler.Receive(pictureByteSize, 0, sizeof(int), SocketFlags.None, out error);
                    if (error != SocketError.Success || received != sizeof(int))
                    {
                        Console.WriteLine("errore " + error.ToString());
                        throw new SocketException();
                    }
                    pictureSize = BitConverter.ToInt32(pictureByteSize, 0);
                    pictureData = new byte[pictureSize];
                    received = 0;

                    while (received < pictureSize)
                    {
                        received += handler.Receive(pictureData, received, pictureSize - received, SocketFlags.None, out error);
                        if (error != SocketError.Success)
                        {
                            Console.WriteLine("errore " + error.ToString());
                            throw new SocketException();
                        }
                    }
                    Console.WriteLine("PictureSize: {0},received {1} ", pictureSize, received);

                    ms = new MemoryStream(pictureData);
                    bitmap = new Bitmap(ms);

                    var barcodeResult = new BarcodeReader().Decode(bitmap);
                    Console.WriteLine("BARCODE {0} ", barcodeResult);
                    if (barcodeResult != null && barcodeResult.BarcodeFormat == BarcodeFormat.QR_CODE)
                    {
                        HttpResponseMessage response = client.GetAsync(Constants.URI + Constants.DOOR_ID + "/" + barcodeResult.Text).Result;
                        Console.WriteLine("RESPONSE CODE {0} ", response.StatusCode);
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine("Response {0}", result);
                            if (String.Compare(result, "true") == 0)
                                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.ACCEPT.ToString());
                            else if (String.Compare(result, "false") == 0)
                                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.REJECT.ToString());
                        }
                    }

                    handler.Send(responseToFEZ, 0, responseToFEZ.Length, SocketFlags.None, out error);
                    if (error != SocketError.Success)
                    {
                        Console.WriteLine("errore " + error.ToString());
                        throw new SocketException();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("InnerException: " + e.InnerException);
                    Console.WriteLine("Error: " + e.Message.ToString());
                    handler.Close();
                    return;
                }
            }
        }

        private IPEndPoint localEndPoint = null;
        private Socket listener = null;
        private HttpClient client = null;
    }
}