using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;

public class ScriptedArmyAttack : IArmyAIAction
{ 
    [SerializeField]
    private ArmyGroupTemplateData m_AttackGroupData;
    public bool isRandomizedAction => false;

    ArmyGroupTemplateData IArmyAIAction.GetAction() => m_AttackGroupData;
}
