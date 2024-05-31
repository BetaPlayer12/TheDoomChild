using System.Collections;
using Cinemachine;
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

        private ICinemachineCamera m_blendCamA;
        private ICinemachineCamera m_blendCamB;
        private CinemachineBasicMultiChannelPerlin m_blendCamANoise;
        private CinemachineBasicMultiChannelPerlin m_blendCamBNoise;

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
                RemoveNoiseFromCamera(m_currentCamera.noiseModule);
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
                    var brain = GameplaySystem.cinema.currentBrain;
                    if (brain.ActiveBlend.IsValid && !brain.ActiveBlend.IsComplete)
                    {
                        HandleShakeDuringCameraBlend(brain);
                    }
                    else
                    {
                        if (brain.ActiveBlend.IsComplete)
                        {
                            if (m_blendCamA != null)
                            {
                                RemoveNoiseFromCamera(m_blendCamANoise);
                                m_blendCamA = null;
                                m_blendCamB = null;
                            }
                        }

                        ApplyNoise(m_currentCamera.noiseModule, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);
                    }
                }
                yield return null;
            } while (m_blendHandle.hasClipsLeft);

            if (m_currentCamera != null)
            {
                RemoveNoiseFromCamera(m_currentCamera.noiseModule);
            }
            m_isExecutingShake = false;
        }

        private void HandleShakeDuringCameraBlend(CinemachineBrain brain)
        {
            VerifyBlendCamera(ref m_blendCamA, m_blendCamANoise, brain.ActiveBlend.CamA);
            VerifyBlendCamera(ref m_blendCamB, m_blendCamBNoise, brain.ActiveBlend.CamB);

            ApplyNoise(m_blendCamANoise, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);
            ApplyNoise(m_blendCamBNoise, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);
        }

        private void VerifyBlendCamera(ref ICinemachineCamera cache, CinemachineBasicMultiChannelPerlin cacheNoise, ICinemachineCamera activeBlendCam)
        {
            if (cache != activeBlendCam)
            {
                RemoveNoiseFromCamera(cacheNoise);
                cache = activeBlendCam;

                cacheNoise = ((CinemachineVirtualCamera)cache).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }

        private void RemoveNoiseFromCamera(CinemachineBasicMultiChannelPerlin noiseModule)
        {
            ApplyNoise(noiseModule, null, 0, 0);
        }

        private void ApplyNoise(CinemachineBasicMultiChannelPerlin module, NoiseSettings profile, float amplitude, float frequency)
        {
            module.m_NoiseProfile = profile;
            module.m_AmplitudeGain = amplitude;
            module.m_FrequencyGain = frequency;
        }

        private void Awake()
        {
            m_blendHandle = new CameraShakeBlendHandle();
        }
    }
}