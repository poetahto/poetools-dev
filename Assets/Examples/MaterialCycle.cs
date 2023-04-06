using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Examples
{
    public class MaterialCycle : NetworkBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        public NetworkVariable<Color> currentColor = new NetworkVariable<Color>();

        public override void OnNetworkSpawn()
        {
            currentColor.OnValueChanged += HandleValueChanged;

            if (IsServer)
                currentColor.Value = meshRenderer.material.color;

            else HandleValueChanged(default, currentColor.Value);
        }

        public override void OnNetworkDespawn()
        {
            currentColor.OnValueChanged -= HandleValueChanged;
        }

        private void HandleValueChanged(Color _, Color value)
        {
            meshRenderer.material.color = value;
        }

        public void RandomizeColor()
        {
            currentColor.Value = Random.ColorHSV();
        }
    }
}
