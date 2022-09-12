using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Environment
{
    public class StickWhileWallStickPlatform : MonoBehaviour, IPlayerWallStickPlatformReaction
    {
        [SerializeField]
        private Transform m_toParent;

        public void ReactToPlayerWallStick(Character player)
        {
            player.transform.SetParent(m_toParent);
        }

        public void ReactToPlayerWallUnstick(Character player)
        {
            player.transform.SetParent(null);
            SceneManager.MoveGameObjectToScene(player.gameObject, GameplaySystem.playerManager.player.gameObject.scene);
        }
    }
}
