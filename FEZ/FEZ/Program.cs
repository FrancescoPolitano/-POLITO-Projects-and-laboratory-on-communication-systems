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

        private IPEndPoint remoteEP;
        private Socket sockSender = null;

        void ProgramStarted()
        {
            remoteEP = new IPEndPoint(IPAddress.Parse(Constants.IP_SERVER), Constants.PORT_TCP);
            Debug.Print("PROGRAM STARTED");
            camera.CameraConnected += Camera_CameraConnected;
            camera.CameraDisconnected += Camera_CameraDisconnected;
            camera.PictureCaptured += Camera_PictureCaptured;
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution176x144;
            ethernetJ11D.UseThisNetworkInterface();
            ethernetJ11D.NetworkUp += EthernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += EthernetJ11D_NetworkDown;
            new Thread(connectionChecking).Start();
        }


        private void Camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            try
            {
                byte[] image = e.PictureData;
                Debug.Print("FOTO ACQUISITA");
                int pictureSize = image.Length;
                //TODO CHANGE
                sockSender.ReceiveTimeout = 0;
                sockSender.SendTimeout = 0;
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
                Debug.Print("Exception " + ex.Message);
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
            while (ethernetJ11D.IsNetworkUp == false)
            {
                Debug.Print("Waiting...");
                Thread.Sleep(1000);

            }


            while (true)
            {
                sockSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sockSender.Connect(remoteEP);
                IPEndPoint ipEndPoint = sockSender.RemoteEndPoint as IPEndPoint;
                if (String.Compare(ipEndPoint.Address.ToString(), remoteEP.Address.ToString()) == 0
                    && ipEndPoint.Port == remoteEP.Port)
                    break;
                Debug.Print("ASPETTO IL SERVER");
                sockSender.Close();
                ledStrip.SetBitmask(28);
                Thread.Sleep(30);
            }

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