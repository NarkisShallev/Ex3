﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace Ex3.Models
{
    public class Server
    {
        private TcpListener server;
        private TcpClient connectedClient;
        private BinaryReader reader;
        private Thread thread;

        public bool IsConnected { get; set; }
        private String[] data;
        public String[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        // instance for singleton pattern
        private static Server instance = null;

        private Server()
        {
            this.IsConnected = false;
        }

        // instance method for singleton pattern
        public static Server Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Server();
                }
                return instance;
            }
        }

        // open server
        public void OpenServer(string IP, int port)
        {
            server = new TcpListener(new IPEndPoint(IPAddress.Parse(IP), port));
            server.Start();
            thread = new Thread(() =>
            {
                while (true)
                {
                    if (!IsConnected)
                    {
                        try
                        {
                            connectedClient = server.AcceptTcpClient();
                        }
                        catch (SocketException)
                        {
                            Console.WriteLine("Waiting for connection...");
                            continue;
                        }
                    }
                    reader = new BinaryReader(connectedClient.GetStream());
                    IsConnected = true;
                    ReadFromClient();
                }
            });
            thread.Start();
        }

        // read from client and separate by commas
        public void ReadFromClient()
        {
            String buffer = "";
            char c;
            try
            {
                c = reader.ReadChar();
            }
            catch
            {
                Console.WriteLine("Reading from client failed");
                CloseServer();
                return;
            }
            while (c != '\n')
            {
                buffer += c;
                try
                {
                    c = reader.ReadChar();
                }
                catch
                {
                    Console.WriteLine("Reading from client failed");
                    CloseServer();
                    return;
                }
            }
            Data = buffer.Split(',');
        }

        // close server
        public void CloseServer()
        {
            server.Stop();
            IsConnected = false;
            thread.Abort();
        }
    }
}