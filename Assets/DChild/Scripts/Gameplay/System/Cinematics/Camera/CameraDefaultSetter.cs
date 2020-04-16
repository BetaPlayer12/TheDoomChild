using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class CameraDefaultSetter : MonoBehaviour
    {
        [SerializeField]
        private VirtualCamera m_vCam;

#if UNITY_EDITOR
        public void Set(VirtualCamera camera) => m_vCam = camera;
#endif

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Hitbox")
            {
                var player = collision.GetComponentInParent<Player>();
                if (player != null)
                {
                    GameplaySystem.cinema.SetDefaultCam(m_vCam);
                    GameplaySystem.cinema.TransistionToDefaultCamera();
                }
            }
        }

    }


}