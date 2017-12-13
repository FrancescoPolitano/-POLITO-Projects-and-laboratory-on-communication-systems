using System;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
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
                receiveFromFez(handler);
            }
        }

        private void receiveFromFez(Socket handler)
        {
            while (true)
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
                    if (error != SocketError.Success)
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
                        if (pictureSize - received > Constants.PACKET_SIZE)
                            received += handler.Receive(pictureData, received, Constants.PACKET_SIZE, SocketFlags.None, out error);
                        else received += handler.Receive(pictureData, received, pictureSize - received, SocketFlags.None, out error);

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

                    byte[] responseToFEZ = new byte[Constants.EVALUATION.ACCEPT.ToString().Length];


                    var barcodeReader = new BarcodeReader();
                    //var barcodeBitmap = (Bitmap)Bitmap.FromFile(@"C:\Users\Cristiano\Desktop\jpeg.jpg");

                    var barcodeResult = barcodeReader.Decode(bitmap);
                    if (barcodeResult != null && barcodeResult.BarcodeFormat == BarcodeFormat.QR_CODE)
                    {
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = client.GetAsync("http://192.168.1.171:8082/RestfulService/resources/auth/" + Constants.DOOR_ID + "/" + barcodeResult.Text).Result;
                        Console.WriteLine("RESPONSE CODE {0} ", response.StatusCode);
                        if (response.IsSuccessStatusCode)
                        {
                            string str = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine("str {0}", str);
                            if (String.Compare(str, "true") == 0)
                                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.ACCEPT.ToString());
                            else if (String.Compare(str, "true") == 0)
                                responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.REJECT.ToString());
                        }
                        else responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());
                    }
                    else responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());

                    //Console.WriteLine($"Decoded barcode text: {barcodeResult?.Text}");
                    //Console.WriteLine($"Barcode format: {barcodeResult?.BarcodeFormat}");
                    //responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.REJECT.ToString());
                    //responseToFEZ = Encoding.UTF8.GetBytes(Constants.EVALUATION.NOCODE.ToString());


                    sent = handler.Send(responseToFEZ, 0, responseToFEZ.Length, SocketFlags.None, out error);
                    Console.WriteLine("RESPONSE SENT");
                    if (error != SocketError.Success)
                    {
                        Console.WriteLine("errore " + error.ToString());
                        handler.Close();
                        return;
                    }

                    bitmap.Save(@"C:\Users\Cristiano\Desktop\image.jpeg", ImageFormat.Jpeg);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine("errore " + ae.Message);
                }
                catch (SocketException s)
                {
                    handler.Close();
                    Console.WriteLine("SOCKET CLOSED EXCEPTION");
                    Console.WriteLine("errore " + s.Message);
                    return;
                }

            }
        }

        private IPEndPoint localEndPoint = null;
        private Socket listener = null;
    }
}