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
        public WeaponLevel level => m_level;
        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private WeaponUpgradeRequirement[] m_weaponUpgradeRequirement;
        public WeaponUpgradeRequirement[] weaponUpgradeRequirement => m_weaponUpgradeRequirement;
        [SerializeField]
        private WeaponUpgradeResult m_attackDamage;
        public WeaponUpgradeResult attackdamage => m_attackDamage;

        public bool hasUpgradeRequirements;
        
    }
}
