﻿using System;
using System.Timers;

namespace Ex3.Models
{
    public class DisplayRefreshingLocation
    {
        string ip;
        int port;

        DisplayUtils displayUtils;
        Timer timer;

        public DisplayRefreshingLocation(string ip, int port, int time)
        {
            displayUtils = new DisplayUtils();
            this.ip = ip;
            this.port = port;
            InitializeTimer(time);
        }

        private void InitializeTimer(int time)
        {
            timer = new Timer(1000 / time);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            displayUtils.ReadLatAndLon(ip, port);
        }
    }
}