using HarmonyLib;

namespace FoundryDedicatedTools
{
    [HarmonyPatch]
    public class CreativeMode
    {
        [HarmonyPatch(typeof(SessionSettingsManager), nameof(SessionSettingsManager.Init))]
        static void Postfix()
        {
            var id = GameRoot.getSingleton().sessionSettingsId_creativeMode;
            if (GlobalStateManager.isDedicatedServer && SessionSettingsManager.singleton.dict_sessionSettings.TryGetValue(id, out _))
            {
                SessionSettingsManager.singleton.dict_sessionSettings[id] = 1L;
            }
        }
    }
}
