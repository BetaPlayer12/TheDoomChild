using System.Collections;
using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    [System.Serializable]
    public class CameraShakeHandle : MonoBehaviour
    {
        private IVirtualCamera m_currentCamera;

        public void Execute(CameraShakeInfo data)
        {
            if (data != null)
            {
                StopAllCoroutines();
                StartCoroutine(ExecuteShakeRoutine(data));
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
                camera.noiseModule.m_AmplitudeGain = m_currentCamera.noiseModule.m_AmplitudeGain;
                camera.noiseModule.m_FrequencyGain = m_currentCamera.noiseModule.m_FrequencyGain;
                RemoveNoiseFromCamera(m_currentCamera);
            }
            m_currentCamera = camera;
        }

        private IEnumerator ExecuteShakeRoutine(CameraShakeInfo info)
        {
            var timer = 0f;

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