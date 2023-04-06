using Unity.Netcode;

namespace Examples
{
    public class PlayerSpawnTeleporter : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            NetworkManager.SceneManager.OnSceneEvent += HandleSceneEvent;
        }

        public override void OnNetworkDespawn()
        {
            NetworkManager.SceneManager.OnSceneEvent -= HandleSceneEvent;
        }

        private void HandleSceneEvent(SceneEvent sceneEvent)
        {
            if (sceneEvent.ClientId == OwnerClientId && sceneEvent.SceneEventType == SceneEventType.LoadComplete)
            {
                var spawnPoint = FindObjectOfType<PlayerSpawnPosition>().transform;
                transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
