using System;

namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        static string Save(string[] args)
        {
            GameRoot.getSingleton().QueueAutosave();
            return $"{DateTime.Now:HH:mm:ss} Saved";
        }
    }
}
