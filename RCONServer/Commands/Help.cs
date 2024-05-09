using HarmonyLib;
using System;
using System.Linq;

namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        static string Help(string[] args)
        {
            return Enum.GetNames(typeof(Command)).Join(delimiter: ", ");
        }

    }
}
