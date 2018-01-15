using System;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Diagnostics;

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
            //TODO Utilizzare sent e received sia qui che nella FEZ
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
                //TODO change
                byte[] pictureData;
                byte[] responseToFEZ = new byte[Constants.EVALUATION.ACCEPT.ToString().Length];
                int pictureSize = 0, received = 0, sent = 0;
                SocketError error;
                try
                {
                    handler.ReceiveTimeout = 8000;
                    handler.SendTimeout = 8000;

                    byte[] pictureByteSize = new byte[sizeof(int)];

                    received = handler.Receive(pictureByteSize, 0, sizeof(int), SocketFlags.None, out error);
                    if (error != SocketError.Success || received != sizeof(int))
                    {
                        Console.WriteLine("errore " + error.ToString());
                        handler.Close();
                        return;
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
                            handler.Close();
                            return;
                        }
                    }
                    Console.WriteLine("PictureSize: {0},received {1} ", pictureSize, received);

                    Bitmap bitmap;
                    MemoryStream ms = new MemoryStream(pictureData);
                    bitmap = new Bitmap(ms);

                    var barcodeReader = new BarcodeReader();
                    //var barcodeBitmap = (Bitmap)Bitmap.FromFile(@"C:\Users\Cristiano\Desktop\jpeg.jpg");

                    var barcodeResult = barcodeReader.Decode(bitmap);
                    if (barcodeResult != null && barcodeResult.BarcodeFormat == BarcodeFormat.QR_CODE)
                    {
                        HttpResponseMessage response = client.GetAsync(Constants.URI + Constants.DOOR_ID + "/" + barcodeResult.Text).Result;
                        Console.WriteLine("RESPONSE CODE {0} ", response.StatusCode);
                        if (response.IsSuccessStatusCode)
                        {
                            string str = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine("str {0}", str);
                            if (String.Compare(str, "true") == 0)
                                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.ACCEPT.ToString());
                            else if (String.Compare(str, "false") == 0)
                                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.REJECT.ToString());
                        }
                        else responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());
                    }
                    else responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());

                    //Console.WriteLine($"Decoded barcode text: {barcodeResult?.Text}");
                    //Console.WriteLine($"Barcode format: {barcodeResult?.BarcodeFormat}");


                    sent = handler.Send(responseToFEZ, 0, responseToFEZ.Length, SocketFlags.None, out error);
                    if (error != SocketError.Success)
                    {
                        Console.WriteLine("errore " + error.ToString());
                        handler.Close();
                        return;
                    }

                    bitmap.Save(@"C:\Users\Cristiano\Desktop\image.jpeg", ImageFormat.Jpeg);
                }
                catch (SocketException s)
                {
                    handler.Close();
                    Console.WriteLine("SOCKET CLOSED EXCEPTION");
                    Console.WriteLine("errore " + s.Message);
                    return;
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine("errore " + ae.Message);
                    handler.Close();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine("errore " + e.Message);
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