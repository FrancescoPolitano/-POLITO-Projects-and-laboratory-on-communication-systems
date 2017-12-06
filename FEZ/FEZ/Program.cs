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

namespace FEZ
{
    public partial class Program
    {
        void ProgramStarted()
        {
            camera.CameraDisconnected += Camera_CameraDisconnected;
            camera.CameraConnected += Camera_CameraConnected;
            ethernetJ11D.UseThisNetworkInterface();
            ethernetJ11D.NetworkUp += EthernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += EthernetJ11D_NetworkDown;
            button.ButtonPressed += Button_ButtonPressed;
            camera.PictureCaptured += Camera_PictureCaptured;
            new Thread(RunWebServer).Start();
        }

        private void Camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            Debug.Print("Picture taken");
            byte[] image = e.PictureData;
            int pictureSize = image.Length;
            Debug.Print("Send picture");
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("192.168.100.1"), 8989);
            Socket sockSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sockSender.Connect(remoteEP);
            int sent = sockSender.Send(BitConverter.GetBytes(pictureSize), 0, sizeof(int), SocketFlags.None);
            sent = 0;
            //TODO handle socket exception
            while (sent < pictureSize)
            {
                if (pictureSize - sent > 1400)
                    sent += sockSender.Send(image, sent, 1400, SocketFlags.None);
                else sent += sockSender.Send(image, sent, pictureSize - sent, SocketFlags.None);
                if (sent == pictureSize)
                    Debug.Print("FINISHED");
            }
            sockSender.Close();
            Debug.Print("sent " + sent);
        }

        private void Button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            camera.TakePicture();
        }

        private void Camera_CameraConnected(Camera sender, EventArgs e)
        {
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution176x144;
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
                Thread.Sleep(1000);
            }
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
