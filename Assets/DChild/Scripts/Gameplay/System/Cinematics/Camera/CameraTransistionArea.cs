using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraTransistionArea : MonoBehaviour
    {
        [SerializeField]
        private VirtualCamera m_vCam;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                var player = collision.GetComponentInParent<PlayerControlledObject>();
                if (player != null && player.tag == Character.objectTag)
                {
                    GameplaySystem.cinema.TransistionTo(m_vCam);
                }
            }
        }

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Hitbox"))
        //    {
        //        var player = collision.GetComponentInParent<Player>();
        //        if (player != null)
        //        {
        //            GameplaySystem.cinema.TransistionToDefaultCamera();
        //        }
        //    }
        //}
    }


}