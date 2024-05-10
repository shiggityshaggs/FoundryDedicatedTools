namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        static string DayNightCycle(string[] args)
        {
            if (bool.TryParse(args[0], out bool enabled))
            {
                GameRoot.addLockstepEvent(new GameRoot.DayNightCycleToogleEvent(enabled));
                return $"DayNightCycle is now {enabled}";
            }
            return $"Invalid argument";
        }
    }
}
