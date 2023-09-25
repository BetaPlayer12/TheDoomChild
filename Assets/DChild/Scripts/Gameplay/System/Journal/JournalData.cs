using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems.Journal
{
    [CreateAssetMenu(fileName = "JournalData", menuName = "DChild/Database/Journal Data")]
    public class JournalData : ScriptableObject
    {
        [SerializeField]
        private int m_id;
        [SerializeField]
        private Sprite m_notification;
        [SerializeField]
        private Sprite m_journalItemImage;
        [SerializeField]
        private string m_journalItemName;
        [SerializeField]
        private string m_description;

        public int ID => m_id;
        public Sprite notification => m_notification;
        public Sprite ItemImage => m_journalItemImage;
        public string ItemName => m_journalItemName;
        public string ItemDescription => m_description;
    }
}
