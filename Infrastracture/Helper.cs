namespace Infrastracture
{
    public static class Helper
    {
        public static string AppConfigPath => $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\honeypotter";
        public static string configFileFirewallSettings => $@"{AppConfigPath}\firewallEndpoint.json";
    }
}