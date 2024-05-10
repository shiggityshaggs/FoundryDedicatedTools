using ConsoleTables;

namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        private static string Version()
        {
            return $"{ResourceDB.resourceLinker.version_major}.{ResourceDB.resourceLinker.version_minor}.{ResourceDB.resourceLinker.version_revision}.{AppVersion.GetRevision()}";
        }

        private static string Server(string[] args)
        {
            var table = new ConsoleTable("Server version", "World", "Seed", "Played");
            table.AddRow(Version(), GameRoot.getWorldName(), GameRoot.getMapSeed(), GameRoot.getPlayedTimeInSeconds().secondsToDurationHHMMSS());
            return table.ToMinimalString().TrimEnd('\r', '\n');
        }
    }
}
