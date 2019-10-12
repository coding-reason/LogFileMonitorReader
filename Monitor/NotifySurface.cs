using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogFileMonitor.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
namespace Monitor
{
    public class NotifySurface : INotifySurface
    {
        HubConnection connection;
        public NotifySurface()
        {

            //connection = new HubConnection("http://localhost:5010/logchangehub");
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5010/logchangehub")
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
         
            
        }
        async Task<bool> startConnection()
        {
            if (!started)
            {
                await connection.StartAsync();
                started = true;
            }
            return true;
        }
        private bool started = false;
        
        public async void Notify(int fileId, string[] data)
        {
            connection.StartAsync();
            //var t = startConnection();


            while (connection.State != HubConnectionState.Connected)
            {
                Thread.Sleep(10);
                if (connection.State == HubConnectionState.Disconnected)
                {
                    Console.WriteLine("disconnected");
                }
            }
            await connection.InvokeAsync("AddLines", fileId, data);
            
            //lch.AddLines(fileId, data);
        }
    }
}
