using System.Collections.Generic;
using poetools.Console;
using poetools.Console.Commands;
using poetools.Core;
using poetools.Multiplayer;
using UnityEngine;

namespace Multiplayer
{
    [CreateAssetMenu]
    public class RelayCommand : Command
    {
        private const int MaxPlayers = 2;

        public override string Name => "relay";

        public override IEnumerable<string> AutoCompletions => new[]
        {
            "relay host", "relay connect",
        };

        public override string Help => $@"
<b>Unity Relay Service Integration</b>
    {"relay host".White()} : Starts a new multiplayer session with you as the host. A join code will be outputted for others to join with.
    {"relay connect [join code]".White()} : Joins an existing multiplayer session with the desired join code.
";

        public override async void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length >= 1)
            {
                switch (args[0])
                {
                    case "host":
                        string joinCode = await MultiplayerController.Singleton.RelayStartup.RelayStartHost(MaxPlayers);
                        console.Log("relay", $"Started session with code {joinCode}");
                        GUIUtility.systemCopyBuffer = joinCode;
                        break;
                    case "connect" when args.Length >= 2:
                        await MultiplayerController.Singleton.RelayStartup.RelayStartClient(args[1]);
                        console.Log("relay", "Connected to session.");
                        break;
                }
            }
        }
    }
}
