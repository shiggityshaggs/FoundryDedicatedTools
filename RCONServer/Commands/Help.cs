using ConsoleTables;
using System;
using System.Collections.Generic;

namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        class CommandInfo
        {
            public string args;
            public string description;

            public CommandInfo(string args, string description)
            {
                this.args = args;
                this.description = description;
            }
        }

        static Dictionary<Command, CommandInfo> descriptions = new Dictionary<Command, CommandInfo>()
        {
            { Command.Help, new CommandInfo(args: "", description: "You're looking at it.") },
            { Command.Save, new CommandInfo(args: "", description: "Force an autosave.") },
            { Command.Server, new CommandInfo(args: "", description: "List server info.") },
            { Command.Players, new CommandInfo(args: "", description: "List player info.") },
            { Command.PauseWhenEmpty, new CommandInfo(args: "True/False", description: "Pause the server while no players are connected.") },
            { Command.DayNightCycle, new CommandInfo(args: "True/False", description: "Stop/start the day/night cycle at the current time of day.") },
            { Command.CreativeMode, new CommandInfo(args: "True/False", description: "Enable/disable the creative menu. Does not apply to currently connected players.") },
            { Command.Kick, new CommandInfo(args: "PlayerID", description: "Kick by Player ID.") },
        };

        static string Help(string[] args)
        {
            var table = new ConsoleTable("#", "Command", "Arguments", "Description");

            foreach (string name in Enum.GetNames(typeof(Command)))
            {
                Command command = (Command)Enum.Parse(typeof(Command), name);
                if (descriptions.TryGetValue(command, out CommandInfo info))
                {
                    table.AddRow((int)command, name, info.args, info.description);
                }
            };
            return table.ToMinimalString().TrimEnd('\r', '\n');
        }
    }
}
