using Domain;
using Infrastracture;
using Newtonsoft.Json;

namespace HoneyPotter.Extensions
{
    public static class FirewallSettingsExtension
    {
        public static FirewallSettings Load(this FirewallSettings firewallSettings)
        {
            var serializedData = File.ReadAllText(Helper.configFileFirewallSettings);

            Console.WriteLine("Configuration loaded successful");
            return JsonConvert.DeserializeObject<FirewallSettings>(serializedData);
        }
    }
}
