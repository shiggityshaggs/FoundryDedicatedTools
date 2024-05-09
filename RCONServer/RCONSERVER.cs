using HarmonyLib;
using Rcon;
using Rcon.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FoundryDedicatedTools
{
    [HarmonyPatch]
    public partial class RCONSERVER
    {
        [HarmonyPatch(typeof(GameRoot), "Awake"), HarmonyPostfix]
        private static void Bootstrap()
        {
            Main();
        }

        private static RconServer server;

        public static void Main()
        {
            server = new RconServer("SuperSecurePassword", 25575);
            server.OnClientConnected += Server_OnClientConnected;
            server.OnClientAuthenticated += Server_OnClientAuthenticated;
            server.OnClientDisconnected += Server_OnClientDisconnected;
            server.OnClientCommandReceived += Server_OnClientCommandReceived;
            new Thread(() => { server.Start(); }).Start();
        }

        static void Server_OnClientAuthenticated(object sender, ClientAuthenticatedEventArgs e)
        {
            Console.WriteLine("{0} authenticated", e.Client.Client.LocalEndPoint);
        }

        static void Server_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine("{0} disconnected", e.EndPoint);
        }

        static void Server_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("{0} connected", e.Client.Client.LocalEndPoint);
        }

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

        static readonly Dictionary<Command, Func<string[], string>> funcs = new Dictionary<Command, Func<string[], string>>()
        {
            [Command.Help] = new Func<string[], string>(args => Help(args)),
            [Command.Save] = new Func<string[], string>(args => Save(args)),
            [Command.DayNightCycle] = new Func<string[], string>(args => DayNightCycle(args)),
            [Command.CreativeMode] = new Func<string[], string>(args => CreativeMode(args)),
        };

        internal enum Command
        {
            Help,
            Save,
            DayNightCycle,
            CreativeMode,
        }

        static string Server_OnClientCommandReceived(object sender, ClientSentCommandEventArgs e)
        {
            Console.WriteLine("{0}: {1}", e.Client.Client.LocalEndPoint, e.Command);
            
            var command = new ServerCommand(e.Command);
            return !command.isValid ? "Invalid command" : command.func.Invoke(command.args);
        }
    }
}
