using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Trohpies
{
    [System.Serializable]
    public class TrophyInfo
    {
        [SerializeField]
        private int m_ID;
        [SerializeField]
        private string m_name;
        [SerializeField, PreviewField(100), HorizontalGroup("Split")]
        private Sprite m_icon;
        [SerializeField, PreviewField(100), HorizontalGroup("Split")]
        private Sprite m_lockedIcon;
        [SerializeField,]
        private string m_description;

        public TrophyInfo(int m_ID, string m_name, Sprite m_icon, Sprite m_lockedIcon, string m_description)
        {
            this.m_ID = m_ID;
            this.m_name = m_name;
            this.m_icon = m_icon;
            this.m_lockedIcon = m_lockedIcon;
            this.m_description = m_description;
        }
    }
}