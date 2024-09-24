using UnityEngine;
using System.Collections;
using DChild.Gameplay.Cinematics.Cameras;

namespace DChild.Gameplay.Characters.Enemies
{
    public class FrankyCinemaReaction : MonoBehaviour
    {
        [SerializeField]
        private FrankyAI m_franky;
        [SerializeField]
        private VirtualCamera m_camera;
        [SerializeField]
        private CameraShakeData m_groundSlamShake;

        private void ExecuteCameraShake(CameraShakeData cameraShakeData)
        {
            StopAllCoroutines();
            StartCoroutine(CameraShakeRoutine(cameraShakeData));
        }

        private IEnumerator CameraShakeRoutine(CameraShakeData cameraShakeData)
        {
            m_camera.noiseModule.m_AmplitudeGain = 0;
            m_camera.noiseModule.m_FrequencyGain = 0;
            var time = 0f;

            do
            {
                m_camera.noiseModule.m_AmplitudeGain = cameraShakeData.cameraShakeInfo.GetAmplitude(time);
                m_camera.noiseModule.m_FrequencyGain = cameraShakeData.cameraShakeInfo.GetFrequency(time);
                time += GameplaySystem.time.deltaTime;
                yield return null;
            } while (time <= cameraShakeData.cameraShakeInfo.duration);
            yield return null;

            m_camera.noiseModule.m_AmplitudeGain = 0;
            m_camera.noiseModule.m_FrequencyGain = 0;
        }

    }
}