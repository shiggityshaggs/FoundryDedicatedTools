using System;

namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        static string CreativeMode(string[] args)
        {
            if (bool.TryParse(args[0], out bool enabled))
            {
                var id = GameRoot.getSingleton().sessionSettingsId_creativeMode;
                if (SessionSettingsManager.singleton.dict_sessionSettings.TryGetValue(id, out _))
                {
                    SessionSettingsManager.singleton.dict_sessionSettings[id] = enabled ? 1L : 0L;
                    return $"CreativeMode is now {enabled}";
                }
            }
            return $"Invalid argument";
        }
    }
}
