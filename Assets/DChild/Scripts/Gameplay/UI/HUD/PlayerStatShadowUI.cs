using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    public class PlayerStatShadowUI : StylizedPlayerUI<Sprite>
    {
        [SerializeField, PropertyOrder(0)]
        private Image m_graphic;

        [SerializeField, ListDrawerSettings(OnBeginListElementGUI = "OnAnimationBeginElement", HideAddButton = true, HideRemoveButton = true), PropertyOrder(2)]
        protected Sprite[] m_list;

        protected override Sprite[] list { get => m_list; set => m_list=value; }

        public override void ChangeTo(PlayerUIStyle type)
        {
            m_graphic.sprite = m_list[(int)type];
        }
    }
}