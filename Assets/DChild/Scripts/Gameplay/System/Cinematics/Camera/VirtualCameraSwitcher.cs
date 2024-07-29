using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class VirtualCameraSwitcher : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private int m_awakeIndex;
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true)]
        private VirtualCamera[] m_cameraOptions;

        public void EnableCam(int index)
        {
            if (IsIndexOutOfBounds(index))
            {
                Debug.LogError($"Camera ERROR: Attempt to Enable Camera in {gameObject.name} reached out of bounds");
            }
            else
            {
                m_cameraOptions[index].gameObject.SetActive(true);
            }
        }

        public void SwitchTo(int index)
        {
            if (IsIndexOutOfBounds(index))
            {
                Debug.LogError($"Camera ERROR: Attempt to Switch Camera in {gameObject.name} reached out of bounds");
            }
            else
            {
                for (int i = 0; i < m_cameraOptions.Length; i++)
                {
                    m_cameraOptions[i].gameObject.SetActive(false);
                }
                m_cameraOptions[index].gameObject.SetActive(true);
            }
        }

        private bool IsIndexOutOfBounds(int index)
        {
            return index < 0 || index >= m_cameraOptions.Length;
        }

        private void Awake()
        {
            SwitchTo(m_awakeIndex);
        }
    }

}