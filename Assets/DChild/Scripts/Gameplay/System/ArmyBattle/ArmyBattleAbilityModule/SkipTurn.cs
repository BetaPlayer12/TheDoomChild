using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurn : IArmyAbilityEffect
{
    [SerializeField]
    private bool m_isAppliedToOpponent;
    public void ApplyEffect(Army owner, Army opponent)
    {
        if (m_isAppliedToOpponent)
        {
            ArmyBattleSystem.GetArmyCombatHandleOf(opponent).SkipTurn();
        }
        else
        {
            ArmyBattleSystem.GetArmyCombatHandleOf(owner).SkipTurn();
        }
    }

}
