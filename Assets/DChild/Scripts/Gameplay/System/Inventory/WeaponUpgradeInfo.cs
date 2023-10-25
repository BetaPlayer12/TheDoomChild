using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class WeaponUpgradeInfo
    {
        [SerializeField]
        private WeaponLevel m_level;
        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private WeaponUpgradeRequirement[] m_weaponUpgradeRequirement;
        [SerializeField]
        private bool m_hasUpgradeRequirements;
        [SerializeField]
        private WeaponUpgradeResult m_attackdamage;
        public enum WeaponLevel
        {
            Lv1,
            Lv2,
            Lv3,
            Lv4,
            Lv5
        }
        
    }
}
