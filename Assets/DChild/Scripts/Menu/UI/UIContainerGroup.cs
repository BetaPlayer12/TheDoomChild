using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.UI
{
    public class UIContainerGroup : MonoBehaviour
    {
        [SerializeField]
        private bool m_getAllActiveChildren;
        [SerializeField, DisableIf("m_getAllActiveChildren")]
        private UIContainer[] m_containers;

        public void InstantShow()
        {
            for (int i = 0; i < m_containers.Length; i++)
            {
                m_containers[i].InstantShow();
            }
        }

        public void InstantHide()
        {

            for (int i = 0; i < m_containers.Length; i++)
            {
                m_containers[i].InstantHide();
            }
        }

        public void Show()
        {
            for (int i = 0; i < m_containers.Length; i++)
            {
                m_containers[i].Show();
            }
        }

        public void Hide()
        {

            for (int i = 0; i < m_containers.Length; i++)
            {
                m_containers[i].Hide();
            }
        }

        private void Awake()
        {
            if (m_getAllActiveChildren)
            {
                m_containers = GetComponentsInChildren<UIContainer>(false);
            }
        }
    }

}