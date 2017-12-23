using Gadgeteer.Modules.GHIElectronics;
using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
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
            //remoteEP = new IPEndPoint(IPAddress.Parse(Constants.IP_SERVER), Constants.PORT_TCP);
            remoteEP = new IPEndPoint(IPAddress.Parse(Constants.STATIC_IP_SERVER), Constants.PORT_TCP);
            Debug.Print("PROGRAM STARTED");
            camera.CameraConnected += Camera_CameraConnected;
            camera.CameraDisconnected += Camera_CameraDisconnected;
            camera.PictureCaptured += Camera_PictureCaptured;
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution176x144;
            wifiRS21.NetworkDown += WifiRS21_NetworkDown;
            wifiRS21.NetworkUp += WifiRS21_NetworkUp;
            new Thread(connectionChecking).Start();
            initializeWifi();

            //ethernetJ11D.UseThisNetworkInterface();
            //ethernetJ11D.NetworkUp += EthernetJ11D_NetworkUp;
            //ethernetJ11D.NetworkDown += EthernetJ11D_NetworkDown;

        }

        private void initializeWifi()
        {
            ledStrip.SetBitmask(28);
            if(!wifiRS21.NetworkInterface.Opened)
            wifiRS21.NetworkInterface.Open();
            wifiRS21.UseStaticIP(Constants.STATIC_IP, Constants.MASK, Constants.STATIC_IP);

            wifiRS21.NetworkInterface.Join(Constants.WIFI_SSID, Constants.WIFI_PASSWORD);
            while (wifiRS21.NetworkInterface.IPAddress == "0.0.0.0")
            {
                Debug.Print("ASPETTANDO IP");
                Thread.Sleep(200);
            }
            Debug.Print("IP ADDRESS " + wifiRS21.NetworkInterface.IPAddress);
            ledStrip.TurnAllLedsOff();
            //TODO USE STATIC ADDRESS sulla SCHEDA WIFI PC
        }

        private void WifiRS21_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("WIFI UP");
        }

        private void WifiRS21_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("WIFI DOWN");
        }

        private void Camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            try
            {
                byte[] image = e.PictureData;
                int pictureSize = image.Length;
                //TODO CHANGE
                sockSender.ReceiveTimeout = 8000;
                sockSender.SendTimeout = 8000;
                int sent = 0, received = 0;
                sent = sockSender.Send(BitConverter.GetBytes(pictureSize), 0, sizeof(int), SocketFlags.None);
                sent = 0;
                Debug.Print("MANDO FOTO");
                while (sent < pictureSize)
                {
                    if (pictureSize - sent > Constants.PACKET_SIZE)
                        sent += sockSender.Send(image, sent, Constants.PACKET_SIZE, SocketFlags.None);
                    else sent += sockSender.Send(image, sent, pictureSize - sent, SocketFlags.None);
                }
                Debug.Print("FOTO FINITA");

                byte[] responseFromServer = new byte[Constants.ACCEPT.Length];
                received = sockSender.Receive(responseFromServer, 0, responseFromServer.Length, SocketFlags.None);
                Debug.Print("RECEIVED FROM SERVER");
                string responseString = new string(Encoding.UTF8.GetChars(responseFromServer));
                if (String.Compare(responseString, Constants.ACCEPT) == 0)
                {
                    //TODO CHANGE LED BEHAVIOUR
                    ledStrip.SetBitmask(3);
                }
                else if (String.Compare(responseString, Constants.REJECT) == 0)
                {
                }
                else if (String.Compare(responseString, Constants.NOCODE) == 0)
                {
                    ledStrip.SetBitmask(96);
                    Thread.Sleep(100);
                    ledStrip.TurnAllLedsOff();
                }
            }
            catch (SocketException ex)
            {
                //Debug.Print("Exception " + ex.Message);
                Debug.Print("SOCKET EXCEPTION DURANTE LA RICEZIONE");
                sockSender.Close();
                connectionChecking();
            }
            finally
            {
                //TODO TUNING SLEEP
                Thread.Sleep(300);
                if (camera.CameraReady)
                    camera.TakePicture();
            }
        }

        private void connectionChecking()
        {
            //while (ethernetJ11D.IsNetworkUp == false)
            //{
            //    Debug.Print("Waiting...");
            //    Thread.Sleep(1000);

            //}

            while (wifiRS21.IsNetworkUp == false)
            {
                Debug.Print("Waiting...");
                ledStrip.SetBitmask(28);
                Thread.Sleep(1000);
            }


            while (true)
            {
                bool isConnected = true;
                try
                {
                    sockSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ledStrip.SetBitmask(28);
                    sockSender.Connect(remoteEP);
                }
                catch (SocketException e)
                {
                    isConnected = false;
                    Debug.Print("ECCEZIONE DURANTE CONNECT");
                    if (e.ErrorCode == 11003)
                    {
                        //succede solo se stacco il wifi e poi nn riparte
                    }
                    //TODO Risolvere error code 11003
                    //TODO aggiungere più controlli in initializeWifi
                }
                if (isConnected)
                    break;
                Thread.Sleep(400);

            }

            //IPEndPoint ipEndPoint = sockSender.RemoteEndPoint as IPEndPoint;
            //if (String.Compare(ipEndPoint.Address.ToString(), remoteEP.Address.ToString()) == 0
            //    && ipEndPoint.Port == remoteEP.Port)
            //    break;
            //Debug.Print("ASPETTO IL SERVER");
            //sockSender.Close();
            //ledStrip.SetBitmask(28);
            //Thread.Sleep(30);


            ledStrip.TurnAllLedsOff();

            while (!camera.CameraReady)
            {
                Debug.Print("CAMERA NOT READY");
                Thread.Sleep(30);
            }

            camera.TakePicture();
        }


        private void Camera_CameraConnected(Camera sender, EventArgs e)
        {
            Debug.Print("Camera connessa");
        }

        private void Camera_CameraDisconnected(Camera sender, EventArgs e)
        {
            Debug.Print("Camera disconnessa");
        }

        private void EthernetJ11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network is down!");
        }

        private void EthernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network is up!");
        }
    }
}