using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastracture;
using Domain;

namespace HoneyPotter.Listener
{
    public class Server
    {
        private List<IPAddress> _banList = new List<IPAddress>();
        private readonly int _port;
        private readonly IPAddress _iPAddress;
        private readonly OpnSenseClient _opnSenseClient;

        public Server(int port, IPAddress iPAddress, FirewallSettings settings)
        {
            _port = port;
            _iPAddress = iPAddress;

            _opnSenseClient = new OpnSenseClient(settings);
            PopulateIpFromFirewall();
        }

        private void PopulateIpFromFirewall()
        {
            var response = _opnSenseClient.GetBlockList();

            var alias = response.Aliases.First();

            var ipAddressList = alias.Content.Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (ipAddressList.Length > 0)
            foreach (var ip in ipAddressList)
            {
                _banList.Add(IPAddress.Parse(ip));
            }
        }

        public void Start()
        {
            var server = new TcpListener(_iPAddress, _port);
            server.Start();

            while (true)
            {
                using TcpClient newClient = server.AcceptTcpClient();

                IPEndPoint iPEndPoint = newClient.Client.RemoteEndPoint as IPEndPoint;

                Console.WriteLine($"Incomming connection from: {iPEndPoint.Address}");

                if (_banList.Any(x => x.Equals(iPEndPoint.Address)))
                {
                    Console.WriteLine($"ip: {iPEndPoint.Address} already exists in banlist");
                    newClient.Close();
                    continue;
                }
                
                _banList.Add(iPEndPoint.Address);
                _opnSenseClient.BlockIp(_banList);

                newClient.Close();
            }
        }
    }
}
