using poetools.player.Player;
using poetools.player.Player.Interaction;
using Unity.Netcode;
using UnityEngine;

namespace Examples
{
    public class NetworkInteraction : NetworkBehaviour, IInputProvider
    {
        [SerializeField]
        private FPSInteractionLogicContainer container;

        public bool Active { get; set; }

        private void Update()
        {
            var viewRay = new Ray(container.viewDirection.position, container.viewDirection.forward);

            if (IsLocalPlayer && Active && container.PollWantsToInteract())
                InteractServerRpc(viewRay);

            container.InteractionLogic.ViewRay = viewRay;
        }

        [ServerRpc]
        private void InteractServerRpc(Ray ray)
        {
            container.InteractionLogic.ViewRay = ray;
            container.InteractionLogic.Interact(gameObject);
        }
    }
}
