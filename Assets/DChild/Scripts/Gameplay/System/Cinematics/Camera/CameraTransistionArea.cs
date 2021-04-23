using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraTransistionArea : MonoBehaviour
    {
        [SerializeField]
        private VirtualCamera m_vCam;

        private Collider2D m_collider;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                var player = collision.GetComponentInParent<PlayerControlledObject>();
                if (player != null && player.owner.character == GameplaySystem.playerManager.player.character)
                {
                    GameplaySystem.cinema.TransistionTo(m_vCam);
                    m_collider = collision;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_collider == collision)
            {
               GameplaySystem.cinema.ResolveCamTransistion(m_vCam);
            }
        }
    }
}