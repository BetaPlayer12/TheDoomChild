using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class PlayerStatFXUI : StylizedPlayerUI<Canvas>
    {
        [SerializeField, ListDrawerSettings(OnBeginListElementGUI = "OnAnimationBeginElement", HideAddButton = true, HideRemoveButton = true), PropertyOrder(2)]
        protected Canvas[] m_list;

        protected override Canvas[] list { get => m_list; set => m_list = value; }

        private Canvas m_currentCanvas;

        public override void ChangeTo(PlayerUIStyle type)
        {
            if (m_currentCanvas != null)
            {
                m_currentCanvas.enabled = false;
            }

            m_currentCanvas = m_list[(int)type];
            m_currentCanvas.enabled = true;
        }

        private void Awake()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                var canvas = m_list[i];
                if (canvas != null)
                {
                    canvas.enabled = false;
                }
            }
        }
    }
}