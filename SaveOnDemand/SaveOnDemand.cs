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
                    }
                }
                yield return null;
            }
        }
    }
}
