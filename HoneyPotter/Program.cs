﻿using Domain;
using HoneyPotter.Extensions;
using HoneyPotter.Listener;
using Infrastracture;
using Newtonsoft.Json;
using System.Net;

namespace HoneyPotter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new FirewallSettings();
            if (!File.Exists(Helper.configFileFirewallSettings))
            {
                if(!Directory.Exists(Helper.AppConfigPath))
                    Directory.CreateDirectory(Helper.AppConfigPath);

                var configManager = new ConfigurationManager();
                Console.WriteLine("Config is missing, start configuration first");

                settings.EndPoint = configManager.GetInput("Enter endpoint:", false);
                settings.Key = configManager.GetInput("Enter key for enpoint:", false);
                settings.Secret = configManager.GetInput("Enter key for enpoint:", false);

                var serialized = JsonConvert.SerializeObject(settings);

                File.WriteAllText(Helper.configFileFirewallSettings, serialized);
            }

            settings = settings.Load();

            var server = new Server(80, IPAddress.Any, settings);
            server.Start();
        }
    }
}