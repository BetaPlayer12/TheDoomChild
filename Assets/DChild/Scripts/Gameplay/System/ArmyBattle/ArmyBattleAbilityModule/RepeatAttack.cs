using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatAttack : IArmyAbilityEffect
{
    [SerializeField]
    private bool m_isAppliedToOpponent;
    [SerializeField]
    private int m_extraAttack;
    public void ApplyEffect(Army owner, Army opponent)
    {
        if (m_isAppliedToOpponent)
        {
            //ArmyBattleSystem.GetArmyCombatHandleOf(opponent).AddExtraAttack(m_extraAttack);
        }
        else
        {
           // ArmyBattleSystem.GetArmyCombatHandleOf(owner).AddExtraAttack(m_extraAttack);
        }
    }
}
