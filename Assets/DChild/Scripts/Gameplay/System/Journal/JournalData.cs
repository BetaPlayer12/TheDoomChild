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
        private string m_itemName;

        [SerializeField]
        private string m_itemDescription;

        [SerializeField]
        private Material m_material;

        public int ID => m_id;
        
        public Sprite notification => m_notification;

        public string itemName => m_itemName;

        public string itemDescription => m_itemDescription;

        public Material material => m_material;
    }
}
