using System;
using Models;
using Monitor;

namespace LogFileMonitorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Repo();
            r.start();
        }
    }
}
