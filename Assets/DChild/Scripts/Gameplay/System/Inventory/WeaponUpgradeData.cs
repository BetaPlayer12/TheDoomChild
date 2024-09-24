using DChild.Gameplay.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUpgradeData", menuName = "DChild/Gameplay/Trade/WeaponUpgradeData")]
[System.Serializable]
public class WeaponUpgradeData : ScriptableObject
{
    [SerializeField]
    private WeaponUpgradeInfo m_info;
    public WeaponUpgradeInfo info => m_info;
}
