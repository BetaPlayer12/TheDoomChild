using Holysoft.Event;
using UnityEngine;

namespace Holysoft.UI
{
    [RequireComponent(typeof(UITransistion))]
    public class UITransistionCanvas : UICanvas
    {
        [SerializeField]
        private UITransistion m_transistion;

        public UITransistion transistion => m_transistion;

        public override void Show()
        {
            base.Show();
            if (Application.isPlaying)
            {
                m_transistion.Play();
            }
            else
            {
                m_transistion.startOnAwake = true;
            }
        }

        public override void Hide()
        {
            base.Hide();
            m_transistion.startOnAwake = false;
        }

        private void OnTransistionEnd(object sender, EventActionArgs eventArgs)
        {
            Hide();
            m_transistion.Stop();
        }

        private void Awake()
        {
            m_transistion.TransistionEnd += OnTransistionEnd;
        }

        private void OnValidate()
        {
            m_canvas = GetComponent<Canvas>();
            m_transistion = GetComponent<UITransistion>();
        }
    }
}