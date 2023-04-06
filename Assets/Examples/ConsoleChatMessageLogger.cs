using poetools.Console;
using poetools.Multiplayer;
using UnityEngine;

namespace Examples
{
    /// <summary>
    /// Simple binding between the multiplayer chat system and the runtime console.
    /// </summary>
    public class ConsoleChatMessageLogger : MonoBehaviour
    {
        [SerializeField]
        private MultiplayerController multiplayerController;

        private void OnEnable()
        {
            multiplayerController.Initialized += HandleMultiplayerInitialized;
        }

        private void OnDisable()
        {
            multiplayerController.Initialized -= HandleMultiplayerInitialized;
        }

        // We can only bind to multiplayer systems if multiplayer has been started.
        private void HandleMultiplayerInitialized()
        {
            multiplayerController.ChatSystem.MessageReceived += HandleMethodReceived;
        }

        private void HandleMethodReceived(string message)
        {
            RuntimeConsole.Singleton.Log("chat", message);
        }
    }
}
