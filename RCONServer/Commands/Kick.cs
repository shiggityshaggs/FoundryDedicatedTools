namespace FoundryDedicatedTools
{
    public partial class RCONSERVER
    {
        public static string Kick(string[] args)
        {
            if (uint.TryParse(args[0], out var clientid))
            {
                var player = CharacterManager.serverOnly_getByClientId(clientid);
                if (player != null)
                {
                    GameRoot.server_kickClientId(clientid);
                    return $"Player '{player.username}' was kicked from server";
                }
                return $"Player ID {clientid} not found";
            }
            return $"Invalid argument";
        }
    }
}
