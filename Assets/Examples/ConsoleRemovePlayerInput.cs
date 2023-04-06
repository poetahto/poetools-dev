using poetools.Console;
using poetools.player.Player;
using UnityEngine;

namespace Examples
{
    public class ConsoleRemovePlayerInput : MonoBehaviour
    {
        private void OnEnable()
        {
            RuntimeConsole.Singleton.View.OnVisibilityChanged += HandleVisibilityChanged;
        }

        private void OnDisable()
        {
            RuntimeConsole.Singleton.View.OnVisibilityChanged -= HandleVisibilityChanged;
        }

        private void HandleVisibilityChanged(bool prev, bool cur)
        {
            foreach (var inputProvider in GetComponentsInChildren<IInputProvider>())
                inputProvider.Active = !cur;
        }
    }
}
