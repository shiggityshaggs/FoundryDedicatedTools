using Beryl;
using HarmonyLib;
using Newtonsoft.Json;
using Rcon;
using Rcon.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace FoundryDedicatedTools
{
    [HarmonyPatch]
    public partial class RCONSERVER
    {
        [HarmonyPatch(typeof(BerylGameServer), MethodType.Constructor)]
        private static void Prefix()
        {
            Main();
        }

        private static RconServer server;
        public static CubeSavegame savegame;
        public static int nextAutosaveId = -1;

        class Config
        {
            public string Password { get; set; }
            public int Port { get; set; }
            public string ListenIP { get; set; }
        }

        private static Config config;

        public static void Main()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods", "RCONServer", "config.json");
                FileInfo file = new FileInfo(path);
                if (!file.Exists)
                {
                    Console.WriteLine($"RCON {file.FullName} not found");
                    return;
                }

                var json = File.ReadAllText(file.FullName);
                config = JsonConvert.DeserializeObject<Config>(json);
                if (config == null) { throw new Exception($"Failed to deserialize {file.FullName}"); }
                server = new RconServer(config.Password, config.Port, IPAddress.Parse(config.ListenIP));
            }
            catch (Exception e)
            {
                Console.WriteLine($"RCON {e.Message}");
                return;
            }

            Console.WriteLine($"RCON listening on {config.ListenIP}:{config.Port}");

            server.OnClientConnected += Server_OnClientConnected;
            server.OnClientAuthenticated += Server_OnClientAuthenticated;
            server.OnClientDisconnected += Server_OnClientDisconnected;
            server.OnClientCommandReceived += Server_OnClientCommandReceived;
            new Thread(() => { server.Start(); }).Start();
        }

        static void Server_OnClientAuthenticated(object sender, ClientAuthenticatedEventArgs e)
        {
            Console.WriteLine("RCON {0} authenticated", e.Client.Client.LocalEndPoint);
        }

        static void Server_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine("RCON {0} disconnected", e.EndPoint);
        }

        static void Server_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("RCON {0} connected", e.Client.Client.LocalEndPoint);
        }

        static string Server_OnClientCommandReceived(object sender, ClientSentCommandEventArgs e)
        {
            Console.WriteLine("RCON {0}: {1}", e.Client.Client.LocalEndPoint, e.Command);

            var command = new ServerCommand(e.Command);
            return !command.isValid ? $"Invalid command{Environment.NewLine}" : command.func.Invoke(command.args) + Environment.NewLine;
        }
        internal enum Command
        {
            Help,
            Save,
            Server,
            Players,
            PauseWhenEmpty,
            DayNightCycle,
            CreativeMode,
            Kick,
        }

        static readonly Dictionary<Command, Func<string[], string>> funcs = new Dictionary<Command, Func<string[], string>>()
        {
            [Command.Help] = new Func<string[], string>(args => Help(args)),
            [Command.Save] = new Func<string[], string>(args => Save(args)),
            [Command.Server] = new Func<string[], string>(args => Server(args)),
            [Command.Players] = new Func<string[], string>(args => Players(args)),
            [Command.PauseWhenEmpty] = new Func<string[], string>(args => PauseWhenEmpty(args)),
            [Command.DayNightCycle] = new Func<string[], string>(args => DayNightCycle(args)),
            [Command.CreativeMode] = new Func<string[], string>(args => CreativeMode(args)),
            [Command.Kick] = new Func<string[], string>(args => Kick(args)),
        };

        internal class ServerCommand
        {
            public readonly string commmand;
            public readonly Command Enum;
            public readonly Func<string[], string> func;
            public readonly bool isValid;
            public readonly string prefix;
            public readonly string[] args = new string[1] { string.Empty };
            public ServerCommand(string command)
            {
                commmand = command;

                var split = command.Split(new char[] {' '}, 2);
                prefix = split[0];
                if (split.Length > 1) { args = split[1].Split(' '); }

                if (System.Enum.TryParse(value: prefix, ignoreCase: true, result: out Enum))
                {
                    isValid = funcs.TryGetValue(Enum, out func);
                }
            }
        }
    }
}
