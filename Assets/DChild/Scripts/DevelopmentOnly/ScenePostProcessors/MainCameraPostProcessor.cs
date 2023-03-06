#if UNITY_EDITOR
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildEditor
{
    [System.Serializable]
    public class MainCameraPostProcessor : IScenePostProcessor
    {
        [SerializeField]
        private Camera m_camera;

        public MainCameraPostProcessor(Camera camera)
        {
            m_camera = camera;
        }

        [Button]
        public void Execute()
        {
            m_camera.enabled = true;
            m_camera.gameObject.SetActive(true);
            m_camera.GetComponent<CinemachineBrain>().enabled = true;
        }
    }
}
#endif