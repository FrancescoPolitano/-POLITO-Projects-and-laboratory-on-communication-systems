using System;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;

namespace ServerFEZ
{
    class Server
    {
        public static void Main(string[] args)
        {
            //new Server();
            var barcodeReader = new BarcodeReader();
            var barcodeBitmap = (Bitmap)Bitmap.FromFile(@"C:\Users\Cristiano\Desktop\jpeg.jpg");
            var barcodeResult = barcodeReader.Decode(barcodeBitmap);

            Console.WriteLine($"Decoded barcode text: {barcodeResult?.Text}");
            Console.WriteLine($"Barcode format: {barcodeResult?.BarcodeFormat}");
        }

        public Server()
        {
            localEndPoint = new IPEndPoint(IPAddress.Parse("192.168.100.1"), 8989);
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
            int received = 0;
            byte[] pictureByteSize = new byte[sizeof(int)];
            received = handler.Receive(pictureByteSize, 0, sizeof(int), SocketFlags.None);
            int pictureSize = BitConverter.ToInt32(pictureByteSize, 0);
            Console.WriteLine("PICTURE SIZE {0} ", pictureSize);
            byte[] pictureData = new byte[pictureSize];
            received = 0;
            SocketError error;
            while (received < pictureSize)
            {
                if (pictureSize - received > 1400)
                    received += handler.Receive(pictureData, received, 1400, SocketFlags.None, out error);
                else received += handler.Receive(pictureData, received, pictureSize - received, SocketFlags.None, out error);
            }
            Console.WriteLine("PictureSize: {0},received {1} ", pictureSize, received);
            handler.Close();
            Image x = (Bitmap)((new ImageConverter()).ConvertFrom(pictureData));
            x.Save(@"C:\Users\Cristiano\Desktop\image.jpeg", ImageFormat.Jpeg);
        }

        private IPEndPoint localEndPoint;
        private Socket listener;
    }
}
