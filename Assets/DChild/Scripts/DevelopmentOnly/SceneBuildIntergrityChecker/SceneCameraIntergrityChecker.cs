#if UNITY_EDITOR
using Cinemachine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildDebug
{
    public class SceneCameraIntergrityChecker : IProcessSceneWithReport
    {
        public int callbackOrder => 3;

        public static void ValidateIntegrity()
        {
            var cinemachineBrain = GameObject.FindObjectOfType<CinemachineBrain>();
            if (cinemachineBrain != null && cinemachineBrain.enabled == false)
            {
                cinemachineBrain.enabled = true;
            }

            var cinemachineCameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
            for (int i = 0; i < cinemachineCameras.Length; i++)
            {
                cinemachineCameras[i].enabled = false;
            }
        }

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            ValidateIntegrity();
        }
    }
}
#endif