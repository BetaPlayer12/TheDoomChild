using Cinemachine;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class CameraConfineSensor : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private CameraConfiner m_confiner;
        [SerializeField]
        private CinemachineVirtualCamera m_camera;
        [SerializeField]
        [MinValue(0.1f)]
        private float m_time;

        public void SetCameraConfiner(CameraConfiner confiner)
        {
            m_confiner = confiner;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<Player>();
            if (player)
            {
                GameplaySystem.cinema.camera.TransistionToCamera(m_camera, m_time);
            }
        }
    }
}