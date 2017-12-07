using System;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using System.Text;
using System.IO;

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
            //TODO change
            handler.ReceiveTimeout = 0;
            handler.SendTimeout = 0;
            int received = 0, sent = 0;
            byte[] pictureByteSize = new byte[sizeof(int)];
            received = handler.Receive(pictureByteSize, 0, sizeof(int), SocketFlags.None);
            int pictureSize = BitConverter.ToInt32(pictureByteSize, 0);
            Console.WriteLine("PICTURE SIZE {0} ", pictureSize);
            byte[] pictureData = new byte[pictureSize];
            received = 0;
            SocketError error;
            while (received < pictureSize)
            {
                if (pictureSize - received > Constants.PACKET_SIZE)
                    received += handler.Receive(pictureData, received, Constants.PACKET_SIZE, SocketFlags.None, out error);
                else received += handler.Receive(pictureData, received, pictureSize - received, SocketFlags.None, out error);
            }
            Console.WriteLine("PictureSize: {0},received {1} ", pictureSize, received);

            //CONVERSION WITH ZXING
            //REQUEST TO SERVERWEB
            //RESPONSE FROM SERVERWEB

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
            sent = handler.Send(responseToFEZ, 0, responseToFEZ.Length, SocketFlags.None);
            handler.Close();
            Image x = (Bitmap)((new ImageConverter()).ConvertFrom(pictureData));
            x.Save(@"C:\Users\Cristiano\Desktop\image.jpeg", ImageFormat.Jpeg);
        }

        private IPEndPoint localEndPoint;
        private Socket listener;
    }
}