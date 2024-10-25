using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraTrackingTargetChanger : MonoBehaviour
    {
        [SerializeField]
        private Transform m_trackingTarget;

        private void Start()
        {
            GameplaySystem.cinema.SetTrackingTarget(m_trackingTarget);
        }
    }
}