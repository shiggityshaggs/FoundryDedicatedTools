using HarmonyLib;
using System;

namespace FoundryDedicatedTools
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(GameRoot), nameof(GameRoot.queryCreateSavegame))]
        private static void Postfix(string savegameName)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} {savegameName}");
        }

        [HarmonyPatch(typeof(GlobalStateManager), nameof(GlobalStateManager._loadSavegameInternal))]
        private static void Prefix(CubeSavegame savegame)
        {
            RCONSERVER.savegame = savegame;
        }

        [HarmonyPatch(typeof(GameRoot), nameof(GameRoot.getNextAutoSaveNumber))]
        private static void Postfix(int __result)
        {
            RCONSERVER.nextAutosaveId = (__result + 1) % 5;
        }
    }
}
