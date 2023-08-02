using Domain;
using Infrastracture;
using System.Net;
using System.Net.Sockets;

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

        public async void Start()
        {
            var server = new TcpListener(_iPAddress, _port);
            server.Start();

            Console.WriteLine($"using port: {_port}");
            Console.WriteLine($"Port open successful");

            try
            {
                while (true)
                    await Accept(await server.AcceptTcpClientAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"starting Accepting TcpClients error: {ex.Message}");
            }
        }
        async Task Accept(TcpClient client)
        {
            await Task.Yield();
            try
            {
                using (client)
                {
                    Console.WriteLine();
                    IPEndPoint iPEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                    Console.WriteLine($"Incomming connection from: {iPEndPoint.Address}");

                    if (_banList.Any(x => x.Equals(iPEndPoint.Address)))
                    {
                        Console.WriteLine($"ip: {iPEndPoint.Address} already exists in banlist");
                        client.Close();
                    }

                    _banList.Add(iPEndPoint.Address);
                    _opnSenseClient.BlockIp(_banList);

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Accept client error: {ex.Message}");
            }
        }
    }
}
