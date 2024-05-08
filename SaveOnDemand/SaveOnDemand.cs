using HarmonyLib;
using System;
using System.Collections;

namespace FoundryDedicatedTools
{
    [HarmonyPatch]
    internal class SaveOnDemand
    {
        [HarmonyPatch(typeof(GameRoot), nameof(GameRoot.queryCreateSavegame)), HarmonyPostfix]
        private static void GameRoot_queryCreateSavegame(string savegameName)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} {savegameName}");
        }

        [HarmonyPatch(typeof(GameRoot), "Awake"), HarmonyPostfix]
        private static void GameRoot_Awake()
        {
            GameRoot.getSingleton().StartCoroutine(ReadInput());
        }

        private static IEnumerator ReadInput()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                    if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.S)
                    {
                        GameRoot.getSingleton().QueueAutosave();
                        Dump();
                    }
                }
                yield return null;
            }
        }

        private static void Dump()
        {
            var major = ResourceDB.resourceLinker.version_major.ToString();
            var minor = ResourceDB.resourceLinker.version_minor.ToString();
            var rev = ResourceDB.resourceLinker.version_revision.ToString();
            var appRev = AppVersion.GetRevision().ToString();
            Console.WriteLine($"Major: {major}, Minor: {minor}, Rev: {rev}, appRev: {appRev}");
        }
    }
}
