using Cinemachine;
using DChild;
using DChild.Gameplay.Cinematics.Cameras;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public class ZoneEditor : MonoBehaviour
    {

#if UNITY_EDITOR
      
        [SerializeField]
        [Button]
        private void ValidateMainCamera()
        {
            var mainCam = Camera.main.gameObject;

            var mainCamAudioListener = mainCam.GetComponent<AudioListener>();

            if (mainCamAudioListener)
            {
                Component.DestroyImmediate(mainCamAudioListener);
            }
            if (!mainCam.activeSelf)
            {
                mainCam.SetActive(true);
            }

            var mainCamCinemachineBrain = mainCam.GetComponent(typeof(CinemachineBrain)).gameObject;

            if (mainCamCinemachineBrain)
            {
                if (!mainCamCinemachineBrain.activeSelf)
                {
                    mainCamCinemachineBrain.SetActive(true);
                }
            }
            else
            {
                mainCam.AddComponent(typeof(CinemachineBrain));

            }
            if (mainCam.GetComponent(typeof(MainCamera)))
            {
                var mainCameraComponent = mainCam.GetComponent(typeof(MainCamera)).gameObject;
                if (!mainCameraComponent.activeSelf)
                {
                    mainCameraComponent.SetActive(true);
                }
            }
            else
            {
                mainCam.AddComponent(typeof(MainCamera));
            }
        }

        [SerializeField]
        [Button]
        private void ValidateGameplayVirtualCameras()
        {
            var vCamObjectList = FindObjectsOfType<CinemachineVirtualCamera>();

            for(int i = 0; i < vCamObjectList.Length; i++)
            {
                
                var VCamGameObject = vCamObjectList[i].gameObject;
                var VCamGameObjectName = vCamObjectList[i].gameObject.name.Contains("Timeline");
                if( !VCamGameObjectName )
                {
                    if (VCamGameObject.gameObject)
                    {

                        if (VCamGameObject.TryGetComponent(out CinemachineCameraOffset offset) == false)
                        {
                            var cameraOffset = VCamGameObject.gameObject.AddComponent<CinemachineCameraOffset>();
                            cameraOffset.m_ApplyAfter = CinemachineCore.Stage.Finalize;
                        }

                        if (VCamGameObject.TryGetComponent(out CinemachineNoise noise) == false)
                        {
                            VCamGameObject.gameObject.AddComponent<CinemachineNoise>();
                        }
                    }
                }
              
            }
           
        }

        [SerializeField]
        [Button]
        private void DisableGameplayVcams()
        {
            var vCamObjectList = FindObjectsOfType<CinemachineVirtualCamera>();

            for (int i = 0; i < vCamObjectList.Length; i++)
            {
             
                var VCamGameObject = vCamObjectList[i].gameObject;

                if (VCamGameObject)
                {
                    var disableVCam = VCamGameObject.GetComponent<CinemachineVirtualCamera>();
                    
                    disableVCam.enabled = false;
                }
            }

        }

#endif
    }
}

