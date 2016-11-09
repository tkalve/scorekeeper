using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using ScoreKeeper;
using ScoreKeeper.Helpers;
using ScoreKeeper.Models;

namespace ScoreHub
{
    public class ScoreHub
    {
        public UdpClient ReceiveClient { get; set; }

        public void Start()
        {
            var ip = Network.GetLocalIpAddress();
            var url = $"http://{ip}:8889";
            try
            {
                WebApp.Start<Startup>(url: url);
                Console.WriteLine($"Web server started at {url}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error starting web server: {e.Message}");
            }
            
            // Create client
            ReceiveClient = new UdpClient(8888);
            ReceiveClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            try
            {
                // Start receiving
                ReceiveClient.BeginReceive(new AsyncCallback(Receive), null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            Console.WriteLine("ScoreHub started.");
        }

        public void Stop()
        {
            ReceiveClient.Dispose();
            Console.WriteLine("ScoreHub stopped.");
        }

        private void Receive(IAsyncResult res)
        {
            try
            {
                var remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.88.149"), 8888);
                byte[] received = ReceiveClient.EndReceive(res, ref remoteIpEndPoint);
                ReceiveClient.BeginReceive(new AsyncCallback(Receive), null);

                var game = JsonConvert.DeserializeObject<Game>(Encoding.UTF8.GetString(received));
                GameHub.Instance.CurrentGame = game;
                Console.WriteLine($"Received {received.Length} bytes from {remoteIpEndPoint.Address}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to receive data.");
            }
        }
    }
}