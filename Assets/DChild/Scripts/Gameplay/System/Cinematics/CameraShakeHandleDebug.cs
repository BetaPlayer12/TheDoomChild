using DChild.Gameplay.Cinematics.Cameras;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraShakeHandleDebug : MonoBehaviour
    {
        [SerializeField]
        private CameraShakeData m_data;

        [Button]
        public void ForceShake()
        {
            GameplaySystem.cinema.ExecuteCameraShake(m_data.cameraShakeInfo);
        }
    }
}