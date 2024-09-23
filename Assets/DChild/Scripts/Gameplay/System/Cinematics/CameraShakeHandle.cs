using System.Collections;
using System.Collections.Generic;
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

        private List<IVirtualCamera> m_registeredCamera;

        private CameraShakeInfo m_currentShakeInfo;
        [ShowInInspector, DisableInPlayMode, HideInEditorMode]
        private bool m_isExecutingShake;

        [ShowInInspector, DisableInPlayMode, HideInEditorMode]
        private bool m_isHandlingCameraBlendShake => m_isExecutingShake && GameplaySystem.cinema.currentBrain.ActiveBlend != null;

        [ShowInInspector, DisableInPlayMode, HideInEditorMode]
        public int m_registeredCameraCount => m_registeredCamera.Count;

        public void RegisterCamera(IVirtualCamera camera)
        {
            if (m_registeredCamera.Count > 0)
            {
                var referenceCamera = m_registeredCamera[0];
                camera.noiseModule.m_NoiseProfile = referenceCamera.noiseModule.m_NoiseProfile;
                camera.noiseModule.m_AmplitudeGain = referenceCamera.noiseModule.m_AmplitudeGain;
                camera.noiseModule.m_FrequencyGain = referenceCamera.noiseModule.m_FrequencyGain;
            }
            m_registeredCamera.Add(camera);
        }

        public void UnregisterCamera(IVirtualCamera camera)
        {
            m_registeredCamera.Remove(camera);
            camera.noiseModule.m_NoiseProfile = null;
            camera.noiseModule.m_AmplitudeGain = 0;
            camera.noiseModule.m_FrequencyGain = 0;
        }

        public void ClearRegisteredCameras()
        {
            m_registeredCamera.Clear();
        }

        public void Execute(CameraShakeData data)
        {
            if (data != null)
            {
                if (data.cameraShakeInfo.delay > 0)
                {
                    StartCoroutine(DelayedExecute(data));
                }
                else
                {
                    ExecuteShakeImmidiate(data);
                }
            }
            else
            {
                Debug.LogWarning("There was an attempt to use a null reference as camera shake");
            }
        }

        private void ExecuteShakeImmidiate(CameraShakeData data)
        {
            m_blendHandle.Execute(data);
            if (m_isExecutingShake == false)
            {
                StartCoroutine(ExecuteShakeRoutine());
            }
        }

        public void SetCamera(IVirtualCamera camera)
        {
            if (camera == null || camera.noiseModule == null)
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

        private IEnumerator DelayedExecute(CameraShakeData data)
        {
            yield return new WaitForSeconds(data.cameraShakeInfo.delay);
            ExecuteShakeImmidiate(data);
        }

        private IEnumerator ExecuteShakeRoutine()
        {
            m_isExecutingShake = true;

            do
            {
                m_blendHandle.Update(GameplaySystem.time.deltaTime);
                if (m_currentCamera == null && m_registeredCamera.Count == 0)
                {
                    Debug.LogWarning("WARNING: Camera Shake Handle does not have a reference to a camera to apply Shake");
                }
                else
                {
                    var brain = GameplaySystem.cinema.currentBrain;
                    if (brain.ActiveBlend != null)
                    {
                        HandleShakeDuringCameraBlend(brain);

                        if (brain.ActiveBlend.IsComplete)
                        {
                            if (m_blendCamA != null)
                            {
                                RemoveNoiseFromCamera(m_blendCamANoise);
                                m_blendCamA = null;
                                m_blendCamB = null;
                            }
                            ApplyNoise(m_currentCamera.noiseModule, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);

                            for (int i = 0; i < m_registeredCamera.Count; i++)
                            {
                                ApplyNoise(m_registeredCamera[i].noiseModule, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);
                            }
                        }
                    }
                    else
                    {
                        ApplyNoise(m_currentCamera.noiseModule, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);

                        for (int i = 0; i < m_registeredCamera.Count; i++)
                        {
                            ApplyNoise(m_registeredCamera[i].noiseModule, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);
                        }
                    }
                }
                yield return null;
            } while (m_blendHandle.hasClipsLeft);

            if (m_currentCamera != null)
            {
                RemoveNoiseFromCamera(m_currentCamera.noiseModule);
                for (int i = 0; i < m_registeredCamera.Count; i++)
                {
                    RemoveNoiseFromCamera(m_registeredCamera[i].noiseModule);
                }
            }
            m_isExecutingShake = false;
        }

        private void HandleShakeDuringCameraBlend(CinemachineBrain brain)
        {
            VerifyBlendCamera(ref m_blendCamA, ref m_blendCamANoise, brain.ActiveBlend.CamA);
            VerifyBlendCamera(ref m_blendCamB, ref m_blendCamBNoise, brain.ActiveBlend.CamB);

            ApplyNoise(m_blendCamANoise, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);
            ApplyNoise(m_blendCamBNoise, m_blendHandle.profile, m_blendHandle.amplitude, m_blendHandle.frequency);
        }

        private void VerifyBlendCamera(ref ICinemachineCamera cache, ref CinemachineBasicMultiChannelPerlin cacheNoise, ICinemachineCamera activeBlendCam)
        {
            if (activeBlendCam.VirtualCameraGameObject == null)
                return;

            if (cache != activeBlendCam)
            {
                if (cacheNoise != null)
                {
                    RemoveNoiseFromCamera(cacheNoise);
                }
                cache = activeBlendCam;

                if (cache != null)
                {
                    cacheNoise = ((CinemachineVirtualCamera)cache).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }
                else
                {
                    cacheNoise = null;
                }
            }
        }

        private void RemoveNoiseFromCamera(CinemachineBasicMultiChannelPerlin noiseModule)
        {
            ApplyNoise(noiseModule, null, 0, 0);
        }

        private void ApplyNoise(CinemachineBasicMultiChannelPerlin module, NoiseSettings profile, float amplitude, float frequency)
        {
            if (module == null)
                return;

            module.m_NoiseProfile = profile;
            module.m_AmplitudeGain = amplitude;
            module.m_FrequencyGain = frequency;
        }

        private void Awake()
        {
            m_blendHandle = new CameraShakeBlendHandle();
            m_registeredCamera = new List<IVirtualCamera>();
        }
    }
}