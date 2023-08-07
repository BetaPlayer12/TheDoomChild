using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;

public class ScriptedArmyAttack : IArmyAIAction
{ 
    [SerializeField]
    private ArmyGroupData m_AttackGroupData;
    public bool isRandomizedAction => false;

    ArmyGroupData IArmyAIAction.GetAction() => m_AttackGroupData;
}
