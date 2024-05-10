namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        public static string PauseWhenEmpty(string[] args)
        {
            if (bool.TryParse(args[0], out bool enabled))
            {
                AppCFG.pause_server_when_empty = enabled;
                return $"PauseWhenEmpty is now {enabled}";
            }
            return $"Invalid argument";
        }
    }
}
