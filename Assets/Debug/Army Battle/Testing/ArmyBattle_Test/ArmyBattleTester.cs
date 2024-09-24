//using DChild.Gameplay.ArmyBattle;
//using Holysoft.Event;
//using Sirenix.OdinInspector;
//using System;
//using System.Collections;
//using System.Collections.Generic;
using DChild.Gameplay.ArmyBattle;
using UnityEngine;

public class ArmyBattleTester : MonoBehaviour
{
    [SerializeField]
    private PlayerArmyCompositionCreator m_playerArmy;
    [SerializeField]
    private ArmyCompositionData m_enemyArmy;

    public void StartBattle()
    {
        ArmyBattleSystem.StartNewBattle(m_playerArmy.CreatePlayerArmy(), m_enemyArmy.GenerateArmyCompositionInstance());
    }
}
