using UnityEngine;

namespace DChild.Gameplay.UI
{
    [System.Serializable]
    public class StoreNotificationInfo
    {
        [SerializeField]
        private Sprite m_icon;

        [SerializeField]
        private string m_headerLabel;

        [SerializeField]
        private string m_instructions;

        public Sprite icon => m_icon;
        public string headerLabel => m_headerLabel;
        public string instructions => m_instructions;
    }
}