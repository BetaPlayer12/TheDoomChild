using UnityEngine;

namespace DChild.Gameplay.Systems.Lore
{
    [CreateAssetMenu(fileName = "LoreData", menuName = "DChild/Database/Lore Data")]
    public class LoreData : ScriptableObject
    {
        [SerializeField]
        private Sprite m_topic;
        [SerializeField]
        private string m_alphabethName;
        [SerializeField]
        private string m_baybayinName;
        [SerializeField,TextArea(6,10)]
        private string m_message;
        [SerializeField]
        private string m_author;

        public Sprite topic => m_topic;
        public string alphabethName => m_alphabethName;
        public string baybayinName => m_baybayinName;
        public string message => m_message;
        public string author => m_author;
    }
}