using System.Collections;
using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    [System.Serializable]
    public class CameraShakeHandle : MonoBehaviour
    {
        private IVirtualCamera m_currentCamera;

        private CameraShakeInfo m_currentShakeInfo;

        public void Execute(CameraShakeInfo data)
        {
            if (data != null)
            {
                var currentPriority = m_currentShakeInfo?.priority ?? -1;
                if (currentPriority <= data.priority)
                {
                    StopAllCoroutines();
                    StartCoroutine(ExecuteShakeRoutine(data));
                    m_currentShakeInfo = data;
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

        private IEnumerator ExecuteShakeRoutine(CameraShakeInfo info)
        {
            var timer = 0f;
            m_currentCamera.noiseModule.m_NoiseProfile = info.noiseProfile;
            do
            {
                m_currentCamera.noiseModule.m_AmplitudeGain = info.GetAmplitude(timer);
                m_currentCamera.noiseModule.m_FrequencyGain = info.GetFrequency(timer);
                timer += GameplaySystem.time.deltaTime;
                yield return null;
            } while (timer <= info.duration && m_currentCamera != null);

            RemoveNoiseFromCamera(m_currentCamera);
        }

        private void RemoveNoiseFromCamera(IVirtualCamera camera)
        {
            camera.noiseModule.m_AmplitudeGain = 0;
            camera.noiseModule.m_FrequencyGain = 0;
        }
    }
}