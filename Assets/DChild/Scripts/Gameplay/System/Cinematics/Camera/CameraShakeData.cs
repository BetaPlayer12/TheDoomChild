using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    [CreateAssetMenu(fileName = "CameraShakeData", menuName = "DChild/Gameplay/Camera Shake Data")]
    public class CameraShakeData : ScriptableObject
    {
        [SerializeField, HideLabel]
        private CameraShakeInfo m_cameraShakeInfo;

        public CameraShakeInfo cameraShakeInfo => m_cameraShakeInfo;
    }
}