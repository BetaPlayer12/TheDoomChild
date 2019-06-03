using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "DChild/Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        [SerializeField]
        private string m_name;
        [SerializeField, MinValue(1)]
        private int m_maxQuantity;
        [SerializeField]
        private Sprite m_icon;
        [SerializeField]
        private string m_description;

        public string name => m_name;
        public int maxQuantity => m_maxQuantity;
        public Sprite icon => m_icon;
        public string description => m_description;
    }
}