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

namespace FEZ
{
    public partial class Program
    {
        void ProgramStarted()
        {
            Debug.Print("PROGRAM STARTED");
            camera.CameraConnected += Camera_CameraConnected;
            camera.CameraDisconnected += Camera_CameraDisconnected;
            camera.PictureCaptured += Camera_PictureCaptured;
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution176x144;
            ethernetJ11D.UseThisNetworkInterface();
            ethernetJ11D.NetworkUp += EthernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += EthernetJ11D_NetworkDown;
            new Thread(RunWebServer).Start();
        }


        private void Camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            Socket sockSender = null;
            try
            {
                Debug.Print("PICTURE TAKEN");
                byte[] image = e.PictureData;
                Debug.Print("FOTO ACQUISITA");
                int pictureSize = image.Length;
                //Debug.Print("Send picture");
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(Constants.IP_SERVER), Constants.PORT_TCP);
                sockSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sockSender.Connect(remoteEP);
                //TODO CHANGE
                sockSender.ReceiveTimeout = 0;
                sockSender.SendTimeout = 0;
                int sent = 0, received = 0;
                sent = sockSender.Send(BitConverter.GetBytes(pictureSize), 0, sizeof(int), SocketFlags.None);
                sent = 0;
                //TODO handle socket exception
                Debug.Print("MANDO FOTO");
                while (sent < pictureSize)
                {
                    if (pictureSize - sent > Constants.PACKET_SIZE)
                        sent += sockSender.Send(image, sent, Constants.PACKET_SIZE, SocketFlags.None);
                    else sent += sockSender.Send(image, sent, pictureSize - sent, SocketFlags.None);
                    Thread.Sleep(30);
                }
                Debug.Print("FOTO FINITA");

                byte[] responseFromServer = new byte[Constants.ACCEPT.Length];
                received = sockSender.Receive(responseFromServer, 0, responseFromServer.Length, SocketFlags.None);
                Debug.Print("RECEIVED FROM SERVER");
                string responseString = new string(Encoding.UTF8.GetChars(responseFromServer));
                if (String.Compare(responseString, Constants.ACCEPT) == 0)
                {
                    //TURN ON GREEN LIGHT
                    //Debug.Print("GREEN");
                    ledStrip.SetBitmask(3);
                }
                else if (String.Compare(responseString, Constants.REJECT) == 0)
                {
                    //TURN ON RED LIGHT
                    //Debug.Print("RED");
                }
                else if (String.Compare(responseString, Constants.NOCODE) == 0)
                {
                    //CONTINUE TO LOOP
                    //Debug.Print("NOQRCODE");
                    ledStrip.SetBitmask(96);
                }
            }
            catch (SocketException ex)
            {
                Debug.Print(ex.Message);
                sockSender.Close();
                Camera_PictureCaptured(sender, e);
            }
            sockSender.Close();

            //Debug.Print("sent " + sent);
            Thread.Sleep(300);
            camera.TakePicture();
            Thread.Sleep(300);
        }

        private void Camera_CameraConnected(Camera sender, EventArgs e)
        {
            Debug.Print("Camera connessa");
        }

        private void Camera_CameraDisconnected(Camera sender, EventArgs e)
        {
            Debug.Print("Camera disconnessa");
        }

        private void RunWebServer()
        {
            while (ethernetJ11D.IsNetworkUp == false)
            {
                Debug.Print("Waiting...");
                Thread.Sleep(500);
            }
        }

        private void EthernetJ11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network is down!");
        }

        private void EthernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network is up!");
            while (!camera.CameraReady)
            {
                Debug.Print("SLEEP");
                Thread.Sleep(30);
            }
            Debug.Print("READYYYYYYYYYYYY");
            camera.TakePicture();
        }
    }
}