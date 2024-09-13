using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DisableAttack : IArmyAbilityEffect
{
    [SerializeField]
    private UnitType m_unitType;
    public void ApplyEffect(Army owner, Army opponent)
    {
        Debug.Log("Disabled " + m_unitType);
    }

    
    
}
