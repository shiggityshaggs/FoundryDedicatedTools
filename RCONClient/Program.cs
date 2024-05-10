using Rcon;
using System.Text.Json;

namespace MyRconClient
{
    static class Program
    {
        class Config
        {
            public string Password { get; set; } = string.Empty;
            public int Port { get; set; } = 25575;
            public string IP { get; set; } = string.Empty;
        }

        static Config? ReadConfig()
        {
            FileInfo file = new("config.json");
            if (!file.Exists)
            {
                Console.WriteLine($"Could not find {file.FullName}");
                return null;
            }

            var json = File.ReadAllText(file.FullName);
            Config? config = JsonSerializer.Deserialize<Config>(json);
            if (config == null)
            {
                Console.WriteLine($"Failed to deserialize {file.FullName}");
                return null;
            }
            return config;
        }

        static async Task Main()
        {
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Console.ReadKey();

            Config? config = ReadConfig();
            if (config == null)
            {
                Console.WriteLine("Config is null");
                return;
            }

            RconClient client = new();

            try
            {
                if (await client.ConnectAsync(config.IP, config.Port))
                {
                    await client.AuthenticateAsync(config.Password);
                    if (!client.Authenticated)
                    {
                        Console.WriteLine("Invalid password!");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to connect to {config.IP} on port {config.Port}.");
                    return;
                }
            }
            catch
            {
                Console.WriteLine($"Unable to connect to {config.IP} on port {config.Port}.");
                return;
            }

            Console.WriteLine($"Connected to {config.IP} on port {config.Port}.{Environment.NewLine}");
            string initial = await client.SendCommandAsync("help");
            if (!String.IsNullOrEmpty(initial))
            {
                Console.WriteLine(initial);
            }

            while (client.Connected)
            {
                Console.Write("> ");

                var input = Console.ReadLine();
                if (!String.IsNullOrEmpty(input) && input.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) { return; }
                if (!String.IsNullOrEmpty(input) && input.Equals("cls", StringComparison.CurrentCultureIgnoreCase)) { Console.Clear(); continue; }

                string response;
                try
                {
                    response = await client.SendCommandAsync(input);
                }
                catch
                {
                    Console.WriteLine("Disconnected");
                    return;
                }

                if (!String.IsNullOrEmpty(response))
                {
                    Console.WriteLine(response);
                }
            }
        }
    }
}