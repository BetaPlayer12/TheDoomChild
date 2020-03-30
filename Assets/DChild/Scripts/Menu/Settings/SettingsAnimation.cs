using CinemachineRuleOfThirds;
using Holysoft.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Menu.MainMenu
{
    public class SettingsAnimation : UIElement3D
    {
        [SerializeField]
        private CinemachineVirtualCamera m_camera;

        [SerializeField]
        private Animation m_animator;

        [SerializeField]
        private GameObject m_target;

        public void Initialized()
        {
            m_target.SetActive(true);
            m_camera.transform.localPosition = new Vector2(0,0);
        }

        public void TransistionAnimation()
        {
            m_target.SetActive(false);
            m_animator?.Play();
        }
    }
}
