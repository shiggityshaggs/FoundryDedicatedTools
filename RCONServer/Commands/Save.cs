using System;
using System.IO;

namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        static string Save(string[] args)
        {
            GameRoot.getSingleton().QueueAutosave();

            string worldName = GameRoot.getWorldName();
            if (!string.IsNullOrEmpty(AppCFG.server_world_name))
            {
                worldName = SaveManager.removeIllegalCharactersForSaveAndWorldNames(worldName);
            }
            if (RCONSERVER.nextAutosaveId == -1 && savegame != null) RCONSERVER.nextAutosaveId = (savegame.nextAutosaveId + 1) % 5;
            string path = Path.Combine(SaveManager.getFolderPathSavegames(), worldName, $"_autosave_{RCONSERVER.nextAutosaveId}");

            return $"{DateTime.Now:HH:mm:ss} {path}";
        }
    }
}
