using Domain;
using Domain.Requests;
using Domain.Responses;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Immutable;
using System.Net;

namespace Infrastracture
{
    public class OpnSenseClient
    {
        private readonly FirewallSettings _fireWallSettings;
        private readonly RestClientOptions _clientSettings;

        private ResponseAlias _responseAlias;

        public OpnSenseClient(FirewallSettings fireWallSettings)
        {
            _fireWallSettings = fireWallSettings;

            _clientSettings = new RestClientOptions(_fireWallSettings.EndPoint);
            _clientSettings.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true; //Ignores SSL Cert, only for testing!
            _clientSettings.Authenticator = new HttpBasicAuthenticator(fireWallSettings.Key, fireWallSettings.Secret);
        }

        public ResponseAlias GetBlockList()
        {
            var client = new RestClient(_clientSettings);
            var request = new RestRequest("firewall/alias/searchItem");

            request.AddBody(new 
            { 
                current = 1,
                rowCount = 7,
                searchPhrase = "localblocklist"
            });

            var response = client.Post(request);

            if (!response.IsSuccessful) return null;

            var responseAlias = JsonConvert.DeserializeObject<ResponseAlias>(response.Content);

            if (responseAlias.Aliases.Count == 0 || responseAlias.Aliases.Count > 1) return null;

            return _responseAlias = responseAlias;
        }

        public void BlockIp(IList<IPAddress> ipAddresses)
        {
            var alias = _responseAlias.Aliases.First();
            string content = string.Empty;

            for (int i = 0; i < ipAddresses.Count(); i++)
            {
                if (i > 0)
                    content += "\n";

                content += $"{ipAddresses[i]}";
            }
            
            var client = new RestClient(_clientSettings);
            var request = new RestRequest($"firewall/alias/setItem/{alias.Id}");

            var requestAlias = new RequestAlias();
            requestAlias.Alias = new Alias();
            requestAlias.Alias.Id = alias.Id;
            requestAlias.Alias.Enabled = alias.Enabled;
            requestAlias.Alias.Name = alias.Name;
            requestAlias.Alias.Type = "host";
            requestAlias.Alias.Proto = "";
            requestAlias.Alias.Categories = "";
            requestAlias.Alias.Updatefreq = "";
            requestAlias.Alias.NetworkInterface = "";
            requestAlias.Alias.Counters = "0";
            requestAlias.Alias.Description = alias.Description;
            requestAlias.Alias.Content = content;

            requestAlias.Network_content = "";
            requestAlias.Authgroup_content = "";

            var jsonData = JsonConvert.SerializeObject(requestAlias);
            request.AddBody(jsonData);
            var response = client.Post(request);

            if (!response.IsSuccessful)
                return;

            ApplyForAlias();
        }

        private void ApplyForAlias()
        {
            var client = new RestClient(_clientSettings);
            var request = new RestRequest($"firewall/alias/set");
            request.AddBody(new
            {
                alias = new { geoip = new { url = "" } }
            });

            var response = client.Post(request);

            if (!response.IsSuccessful) return;

            Thread.Sleep(100);

            request = new RestRequest($"firewall/alias/reconfigure");
            response = client.Post(request);

            if (!response.IsSuccessful) return;
        }
    }
}
