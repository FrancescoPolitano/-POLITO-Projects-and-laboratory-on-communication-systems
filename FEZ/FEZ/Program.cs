using Gadgeteer.Modules.GHIElectronics;
using System;

using System.Threading;
using Microsoft.SPOT;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using System.Net.Sockets;
using System.Net;
using System.Text;
#pragma warning disable CS0612

namespace FEZ
{
    public partial class Program
    {
        private IPEndPoint remoteEP;
        private Socket sockSender = null;

        void ProgramStarted()
        {
            remoteEP = new IPEndPoint(IPAddress.Parse(Constants.STATIC_IP_SERVER), Constants.PORT_TCP);
            camera.PictureCaptured += Camera_PictureCaptured;
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution160x120;
            wifiRS21.NetworkDown += WifiRS21_NetworkDown;
            wifiRS21.NetworkUp += WifiRS21_NetworkUp;
            if (!wifiRS21.NetworkInterface.Opened)
                wifiRS21.NetworkInterface.Open();
            new Thread(connectionChecking).Start();
            initializeWifi();
        }

        private void initializeWifi()
        {
            ledStrip.SetBitmask(28);
            bool joinSuccess = true;
            while (joinSuccess)
            {
                try
                {
                    GHI.Networking.WiFiRS9110.NetworkParameters[] info;
                    info = wifiRS21.NetworkInterface.Scan(Constants.WIFI_SSID);
                    if (info != null && info.Length != 0)
                    {
                        wifiRS21.NetworkInterface.Join(info[0].Ssid, Constants.WIFI_PASSWORD);
                        joinSuccess = true;
                    }
                }
                catch (GHI.Networking.WiFiRS9110.JoinException je)
                {
                    Debug.Print(je.StackTrace);
                    joinSuccess = false;
                }
                catch (InvalidOperationException ex)
                {
                    Debug.Print(ex.StackTrace);
                    break;
                }
            }
            while (wifiRS21.NetworkInterface.IPAddress == "0.0.0.0")
            {
                Debug.Print("WAITING FOR IP");
                Thread.Sleep(100);
            }
            Debug.Print("IP ADDRESS " + wifiRS21.NetworkInterface.IPAddress);
        }

        private void WifiRS21_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("WIFI UP");
        }

        private void WifiRS21_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("WIFI DOWN");
            initializeWifi();
        }

        private void Camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            try
            {
                byte[] image = e.PictureData;
                int pictureSize = image.Length;

                sockSender.ReceiveTimeout = 10000;
                sockSender.SendTimeout = 8000;
                int sent = 0, received = 0;
                sent = sockSender.Send(BitConverter.GetBytes(pictureSize), 0, sizeof(int), SocketFlags.None);
                if (sent != sizeof(int))
                    throw new Exception();
                Debug.Print("SEND PHOTO");
                sent = sockSender.Send(image);
                if (sent != pictureSize)
                    throw new Exception();
                Debug.Print("FINISHED");

                byte[] responseFromServer = new byte[Constants.ACCEPT.Length];
                received = sockSender.Receive(responseFromServer, 0, responseFromServer.Length, SocketFlags.None);
                if (received != responseFromServer.Length)
                    throw new Exception();
                Debug.Print("RECEIVED FROM SERVER");
                string responseString = new string(Encoding.UTF8.GetChars(responseFromServer));
                if (String.Compare(responseString, Constants.ACCEPT) == 0)
                {
                    ledStrip.SetBitmask(3);
                    Thread.Sleep(5000);
                }
                else if (String.Compare(responseString, Constants.REJECT) == 0)
                {
                    ledStrip.SetBitmask(96);
                    Thread.Sleep(1000);
                }
                else if (String.Compare(responseString, Constants.NOCODE) == 0)
                {
                }
                ledStrip.TurnAllLedsOff();

                while (!camera.CameraReady)
                    Thread.Sleep(100);
                camera.TakePicture();
            }
            catch (Exception exc)
            {
                Debug.Print("EXCEPTION " + exc.StackTrace);
                sockSender.Close();
                connectionChecking();
            }
        }
        private void connectionChecking()
        {
            ledStrip.SetBitmask(28);
            while (wifiRS21.IsNetworkUp == false)
            {
                Debug.Print("Waiting wifi...");
                Thread.Sleep(1000);
            }
            bool isConnected = false;
            while (!isConnected)
            {
                try
                {
                    sockSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sockSender.Connect(remoteEP);
                    isConnected = true;
                }
                catch (SocketException e)
                {
                    Debug.Print("ECCEZIONE DURANTE CONNECT " + e.StackTrace);
                }
            }
            ledStrip.TurnAllLedsOff();

            while (!camera.CameraReady)
                Thread.Sleep(100);
            camera.TakePicture();
        }
    }
}