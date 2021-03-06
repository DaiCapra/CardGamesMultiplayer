﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Core;

namespace PokerClient
{
    public class Client
    {
        private const int CONNECTION_TIMEOUT = 5000;
        private const int CONNECTION_TRIES = 5;

        private ListBox statusBox;
        private Listener listener;
        //buffel

        public Client(string ip, int port, ListBox lb)
        {
            listener = new Listener(ip, port);
            statusBox = lb;
        }

        public void SendAuthenticate(string name, string password)
        {
            string[] messages = new string[] {PacketData.ServerType.Login.ToString(), name, password};
            Packet p = new Packet(PacketData.PacketType.ServerMessage.ToString() , messages);
            string s = p.ListToJson(p.PacketInfo);
            listener.Write(s);
        }

        public void IncomingPacket(string message)
        {

        }
        
        public void ConnectToServer()
        {
            if (!listener.Connected)
            {
                statusBox.Items.Add("Connecting to server...");
                Thread t1 = new Thread(listener.BeginConnect);
                t1.Start();
                t1.IsBackground = true;
                t1.Join(CONNECTION_TIMEOUT);

                if (listener.Connected)
                {
                    statusBox.Items.Add("Connected to server!");
                    t1.Abort();
                }
                else
                {
                    statusBox.Items.Add("Connection failed! Re-trying connection...");
                    for (int i = 0; i < CONNECTION_TRIES; i++)
                    {
                        statusBox.Items.Add("try " + (i + 1) + "... ");
                        t1 = new Thread(listener.BeginConnect);
                        t1.Start();
                        t1.Join(CONNECTION_TIMEOUT);

                        if (listener.Connected)
                        {
                            statusBox.Items.Add("Client connected to server!");
                            t1.Abort();
                            break;
                        }

                    }
                    t1.Abort();
                    if (!listener.Connected)
                        statusBox.Items.Add("Client not connected.");
                }
            }
        }
        public Listener Listener
        {
            get { return listener; }
        }
    }
}
