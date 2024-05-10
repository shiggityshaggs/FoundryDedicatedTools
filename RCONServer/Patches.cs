using HarmonyLib;
using System;
using UnityEngine;

namespace FoundryDedicatedTools
{
    [HarmonyPatch]
    internal class Patches
    {
        private static bool SaveQueued;

        [HarmonyPatch(typeof(GameRoot), nameof(GameRoot.QueueAutosave)), HarmonyPrefix]
        private static void GameRoot_QueueAutosave()
        {
            SaveQueued = true;
            Time.timeScale = 1f;
        }

        [HarmonyPatch(typeof(DedicatedServerPausingSystem), "handle"), HarmonyPrefix]
        private static bool DedicatedServerPausingSystem_handle()
        {
            return !SaveQueued;
        }

        [HarmonyPatch(typeof(GameRoot), nameof(GameRoot.queryCreateSavegame)), HarmonyPostfix]
        private static void GameRoot_queryCreateSavegame(string savegameName)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} {savegameName}");
            SaveQueued = false;
        }

        [HarmonyPatch(typeof(GlobalStateManager), nameof(GlobalStateManager._loadSavegameInternal)), HarmonyPrefix]
        private static void GlobalStateManager_loadSavegameInternal(CubeSavegame savegame)
        {
            RCONSERVER.savegame = savegame;
        }

        [HarmonyPatch(typeof(GameRoot), nameof(GameRoot.getNextAutoSaveNumber)), HarmonyPostfix]
        private static void GameRoot_getNextAutoSaveNumber(int __result)
        {
            RCONSERVER.nextAutosaveId = (__result + 1) % 5;
        }
    }
}
