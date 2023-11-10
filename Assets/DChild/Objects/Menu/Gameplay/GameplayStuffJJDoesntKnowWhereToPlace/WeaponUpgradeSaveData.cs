using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public struct WeaponUpgradeSaveData
    {
        [SerializeField]
        private int m_currentWeaponLevel;

        public WeaponUpgradeSaveData(WeaponLevel currentWeaponLevel)
        {
            m_currentWeaponLevel = (int)currentWeaponLevel;
        }

        public WeaponLevel currentWeaponLevel => (WeaponLevel)m_currentWeaponLevel;
    }
}