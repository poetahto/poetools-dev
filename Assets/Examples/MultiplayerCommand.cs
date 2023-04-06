using System.Collections.Generic;
using poetools.Console;
using poetools.Console.Commands;
using poetools.Core;
using poetools.Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    [CreateAssetMenu]
    public class MultiplayerCommand : Command
    {
        private RuntimeConsole _console;
        private MultiplayerController _controller;

        public override string Name => "multiplayer";

        public override string Help => $@"
<b>Interface with NetCode For GameObjects.</b>
    {"multiplayer host [ip] [port] [listen address]".White()} : Starts a new multiplayer session with you as the host. 
    {"multiplayer server [ip] [port] [listen address]".White()} : Starts a new multiplayer session with you as the host. 
    {"multiplayer connect [ip] [port]".White()} : Joins an existing multiplayer session with the desired join code.
    {"multiplayer stop".White()} : Exits or closes the current multiplayer session.
    {"multiplayer info".White()} : Gets information about the current multiplayer session.
";

        public override IEnumerable<string> AutoCompletions => new[]
        {
            "multiplayer host", "multiplayer connect", "multiplayer server", "multiplayer stop", "multiplayer info", "multiplayer load", "multiplayer chat",
        };

        public override async void Execute(string[] args, RuntimeConsole console)
        {
            if (args.Length >= 1)
            {
                _console = console;
                _controller = MultiplayerController.Singleton;

                string ip = args.Length >= 2 ? args[1] : DirectConnectionStartup.DefaultIP;
                ushort port = args.Length >= 3 && ushort.TryParse(args[2], out var parsed) ? parsed : DirectConnectionStartup.DefaultPort;
                string listenAddress = args.Length >= 4 ? args[3] : DirectConnectionStartup.DefaultListenAddress;

                switch (args[0])
                {
                    // Session Management
                    case "connect":
                        _controller.DirectStartup.StartClient(ip, port);
                        break;
                    case "server":
                        _controller.DirectStartup.StartServer(ip, port, listenAddress);
                        break;
                    case "host":
                        _controller.DirectStartup.StartHost(ip, port, listenAddress);
                        break;
                    case "stop":
                        _controller.Shutdown();
                        break;

                    // Utilities
                    case "load" when args.Length >= 2:
                        await _controller.SceneLoader.Load(args[1]);
                        console.Log("multiplayer", "Finished loading.");
                        break;
                    case "chat" when args.Length >= 2:
                        _controller.ChatSystem.SendMessage(ArgumentTools.Combine(args, 1));
                        break;
                    case "info":
                        HandleInfo();
                        break;
                }
            }
        }

        private void HandleInfo()
        {
            var netManager = NetworkManager.Singleton;
            string infoMessage = $@"

<b>Current Session Info</b>
    Is Client: {netManager.IsClient}
    Is Server: {netManager.IsServer}
    Is Host : {netManager.IsHost}
    {"Player Info".White().Italic()}";

            if (netManager.IsServer)
            {
                foreach (var networkClient in netManager.ConnectedClientsList)
                    infoMessage += $"\n\t\t Player ID: {networkClient.ClientId}, Instance Name: {networkClient.PlayerObject.name}{(netManager.LocalClientId == networkClient.ClientId ? " [IS LOCAL PLAYER]" : "")}";
            }

            else infoMessage += $"\n\t\t Local Player ID: {netManager.LocalClient.ClientId}, Local Instance Name: {netManager.LocalClient.PlayerObject.name}\n";

            _console.Log("relay", infoMessage);
        }
    }
}
