using Beryl;
using HarmonyLib;
using System;
using System.Linq;
using System.Net;

namespace FoundryDedicatedTools
{
    [HarmonyPatch]
    internal class ListenIP
    {
        [HarmonyPatch(typeof(SteamPlatformEndPoint), MethodType.Constructor, new Type[] { typeof(ushort) })]
        static void Postfix(ref SteamPlatformEndPoint __instance)
        {
            if (AppCFG.connect_to != string.Empty && IPAddress.TryParse(AppCFG.connect_to, out IPAddress ip))
            {
                byte[] addressBytes = BitConverter.IsLittleEndian ? ip.GetAddressBytes().Reverse().ToArray() : ip.GetAddressBytes();
                __instance.numericalIP = BitConverter.ToUInt32(addressBytes, 0);
                __instance.NetworkingIPAddr.SetIPv4(__instance.numericalIP, __instance.port);
                __instance.NetworkingIdentity.SetIPv4Addr(__instance.numericalIP, __instance.port);
                __instance.ip = AppCFG.connect_to;
                Console.WriteLine($"app.cfg connect_to was set. Listening on {AppCFG.connect_to}:{__instance.port}");
            }
        }
    }
}
