using ConsoleTables;

namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        private static string Players(string[] args)
        {
            if (savegame != null)
            {
                var table = new ConsoleTable("ID", "UserName", "Ping", "X", "Z", "Height");
                foreach (CubeSavegame.CharacterData data in savegame.list_characters)
                {
                    Character c = CharacterManager.getByUsernameHash((ulong)data.usernameHash);
                    if (c != null)
                    {
                        var online = CharacterManager.isCharacterInWorld((ulong)data.usernameHash);
                        var status = online ? c.pingMS.ToString() : "Offline";
                        var id = online ? c.ServerOnly_currentClientId.ToString() : "";
                        table.AddRow(id, c.username, status, (int)c.position.x, (int)c.position.z, (int)c.position.y);
                    }
                }
                return table.ToMinimalString().TrimEnd('\r', '\n');
            }
            return string.Empty;
        }
    }
}
