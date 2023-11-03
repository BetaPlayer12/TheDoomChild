using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DChild.Gameplay.Systems.WeaponUpgradeInfo;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public struct WeaponUpgradeSaveData
    {
        public WeaponLevel currentWeaponLevel;
    }
}