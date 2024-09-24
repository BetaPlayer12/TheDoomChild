using DChild.Gameplay.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class WeaponUpgradeRequirement 
    {
        [SerializeField]
        private ItemData m_item;
        public ItemData item => m_item;
        [SerializeField]
        private int m_amount;
        public int amount => m_amount;

    }
}
