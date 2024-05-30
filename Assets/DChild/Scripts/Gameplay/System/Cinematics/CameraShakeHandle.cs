using System.Collections;
using DChild.Gameplay.Cinematics.Cameras;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{

    [System.Serializable]
    public class CameraShakeHandle : MonoBehaviour
    {
        [ShowInInspector, HideInEditorMode]
        private CameraShakeBlendHandle m_blendHandle;

        private IVirtualCamera m_currentCamera;

        private CameraShakeInfo m_currentShakeInfo;
        [ShowInInspector, DisableInPlayMode, HideInEditorMode]
        private bool m_isExecutingShake;

        public void Execute(CameraShakeData data)
        {
            if (data != null)
            {
                m_blendHandle.Execute(data);
                if (m_isExecutingShake == false)
                {
                    StartCoroutine(ExecuteShakeRoutine());
                }
            }
            else
            {
                Debug.LogWarning("There was an attempt to use a null reference as camera shake");
            }
        }

        public void SetCamera(IVirtualCamera camera)
        {
            if (camera.noiseModule == null)
                return;

            if (m_currentCamera != null)
            {
                camera.noiseModule.m_NoiseProfile = m_currentCamera.noiseModule.m_NoiseProfile;
                camera.noiseModule.m_AmplitudeGain = m_currentCamera.noiseModule.m_AmplitudeGain;
                camera.noiseModule.m_FrequencyGain = m_currentCamera.noiseModule.m_FrequencyGain;
                RemoveNoiseFromCamera(m_currentCamera);
            }
            m_currentCamera = camera;
        }

        private IEnumerator ExecuteShakeRoutine()
        {
            m_isExecutingShake = true;
            do
            {
                m_blendHandle.Update(GameplaySystem.time.deltaTime);
                if (m_currentCamera == null)
                {
                    Debug.LogWarning("WARNING: Camera Shake Handle does not have a reference to a camera to apply Shake");
                }
                else
                {
                    m_currentCamera.noiseModule.m_NoiseProfile = m_blendHandle.profile;
                    m_currentCamera.noiseModule.m_AmplitudeGain = m_blendHandle.amplitude;
                    m_currentCamera.noiseModule.m_FrequencyGain = m_blendHandle.frequency;
                }
                yield return null;
            } while (m_blendHandle.hasClipsLeft);

            if (m_currentCamera != null)
            {
                RemoveNoiseFromCamera(m_currentCamera);
            }
            m_isExecutingShake = false;
        }

        private void RemoveNoiseFromCamera(IVirtualCamera camera)
        {
            camera.noiseModule.m_AmplitudeGain = 0;
            camera.noiseModule.m_FrequencyGain = 0;
        }

        private void Awake()
        {
            m_blendHandle = new CameraShakeBlendHandle();
        }
    }
}