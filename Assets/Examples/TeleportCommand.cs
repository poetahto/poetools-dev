using Unity.Netcode;
using UnityEngine;

namespace poetools.Console.Commands
{
    [CreateAssetMenu]
    public class TeleportCommand : Command
    {
        public override string Name => "teleport";

        public override void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length != 3)
                return;

            var target = NetworkManager.Singleton.LocalClient.PlayerObject;
            float x = float.Parse(args[0]);
            float y = float.Parse(args[1]);
            float z = float.Parse(args[2]);

            target.transform.position = new Vector3(x, y, z);
        }
    }
}
