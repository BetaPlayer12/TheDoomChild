using System.Collections;
using Holysoft;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class PlayerCamera : MonoBehaviour
    {
        #region NonStatic
        [SerializeField]
        private CinemachineBrain m_camera;
        private CameraShake m_shaker;
        private Coroutine m_shakeRotoutine;

        public new CinemachineBrain camera => m_camera;

        private IEnumerator ShakeRoutine(float range, float duration)
        {
            m_shaker.m_Range = range;
            yield return new WaitForSeconds(duration);
            m_shaker.m_Range = 0;
        }

        public void ShakeCamera(float range, float duration)
        {
            if (m_shakeRotoutine != null)
            {
                StopCoroutine(m_shakeRotoutine);
            }
            m_shakeRotoutine = StartCoroutine(ShakeRoutine(range, duration));
        }
        #endregion

        public void TransistionToCamera(CinemachineVirtualCamera toCam, float deltaTime)
        {
            var fromCam = m_camera.ActiveVirtualCamera;
            if (fromCam == null)
            {
                toCam.gameObject.SetActive(true);
            }
            else
            {
                if (fromCam != (ICinemachineCamera)toCam)
                {
                    toCam.OnTransitionFromCamera(m_camera.ActiveVirtualCamera, Vector3.up, deltaTime);
                    fromCam.VirtualCameraGameObject.SetActive(false);
                    toCam.gameObject.SetActive(true);
                }
            }
        }

        public void Shake(float range, float duration) => ShakeCamera(range, duration);

        public void SetScene(Scene scene) => SceneManager.MoveGameObjectToScene(gameObject, scene);
    }
}