using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace DChild.UI
{
    public class BanterNextSubtitleEffect : MonoBehaviour
    {
        [SerializeField]
        private UIContainer m_container;

        private bool m_enableEffect;

        public void Enable()
        {
            m_enableEffect = true;
        }

        public void Disable()
        {
            m_enableEffect = false;
        }

        public void ActivateEffect()
        {
            if (m_enableEffect)
            {
                m_container.InstantHide();
                m_container.Show();
            }
        }
    }

}