﻿using Gadgeteer.Modules.GHIElectronics;
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
        //TODO remove button
        void ProgramStarted()
        {
            Debug.Print("PROGRAM_STARTEDDDDDDDDDDDDDDDDDDD");
            camera.CameraConnected += Camera_CameraConnected;
            camera.CameraDisconnected += Camera_CameraDisconnected;
            camera.PictureCaptured += Camera_PictureCaptured;
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution176x144;
            ethernetJ11D.UseThisNetworkInterface();
            ethernetJ11D.NetworkUp += EthernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += EthernetJ11D_NetworkDown;
            button.ButtonPressed += Button_ButtonPressed;
            new Thread(RunWebServer).Start();

        }


        private void Camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            byte[] image = e.PictureData;
            int pictureSize = image.Length;
            Debug.Print("Send picture");
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(Constants.IP_SERVER), Constants.PORT_TCP);
            Socket sockSender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sockSender.Connect(remoteEP);
            //TODO CHANGE
            sockSender.ReceiveTimeout = 0;
            sockSender.SendTimeout = 0;
            int sent = 0, received = 0;
            sent = sockSender.Send(BitConverter.GetBytes(pictureSize), 0, sizeof(int), SocketFlags.None);
            sent = 0;
            //TODO handle socket exception
            while (sent < pictureSize)
            {
                if (pictureSize - sent > Constants.PACKET_SIZE)
                    sent += sockSender.Send(image, sent, Constants.PACKET_SIZE, SocketFlags.None);
                else sent += sockSender.Send(image, sent, pictureSize - sent, SocketFlags.None);
            }

            byte[] responseFromServer = new byte[Constants.ACCEPT.Length];
            received = sockSender.Receive(responseFromServer, 0, responseFromServer.Length, SocketFlags.None);

            string responseString = new string(Encoding.UTF8.GetChars(responseFromServer));
            if (String.Compare(responseString, Constants.ACCEPT) == 0)
            {
                //TURN ON GREEN LIGHT
                Debug.Print("GREEN");
                ledStrip.SetBitmask(3);
            }
            else if (String.Compare(responseString, Constants.REJECT) == 0)
            {
                //TURN ON RED LIGHT
                Debug.Print("RED");
            }
            else if (String.Compare(responseString, Constants.NOCODE) == 0)
            {
                //CONTINUE TO LOOP
                Debug.Print("NOQRCODE");
                ledStrip.SetBitmask(96);
            }

            sockSender.Close();
            Debug.Print("sent " + sent);
            Thread.Sleep(500);
            camera.TakePicture();
        }

        private void Button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            if (camera.CameraReady)
            {
                Debug.Print("CAMERA READY");
                camera.TakePicture();
            }
            else Debug.Print("CAMERA NOT READY");
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
            Thread.Sleep(500);
            while (!camera.CameraReady)
            {
                Debug.Print("dormo un po'");
                Thread.Sleep(30);
            }
            Debug.Print("READYYYYYYYYYYYY");
            camera.TakePicture();

        }

        private void disconnected(GHI.Usb.Host.BaseDevice.DisconnectedEventHandler d, Microsoft.SPOT.EventArgs e)
        {
            Debug.Print("DEVICE DISCONNECTED");
        }
    }
}