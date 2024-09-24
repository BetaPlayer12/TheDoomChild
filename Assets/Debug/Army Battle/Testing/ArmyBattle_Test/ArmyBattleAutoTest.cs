//using DChild.Gameplay.ArmyBattle;
//using Holysoft.Event;
//using Sirenix.OdinInspector;
//using System;
//using System.Collections;
//using System.Collections.Generic;
using DChild.Gameplay.ArmyBattle;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class ArmyAbilityInjector : MonoBehaviour
{
    [SerializeField]
    private Army m_army;
}

public class ArmyBattleAutoTest : MonoBehaviour
{
    //[SerializeField]
    //private ArmyBattleHandle m_handle;
    //[SerializeField]
    //private ArmyController m_player;

    //[Button]
    //private void StartTest()
    //{
    //    m_handle.StopAllCoroutines();
    //    m_handle.InitializeBattle();
    //    m_handle.StartRound();
    //}

    //private void OnRoundStart(object sender, EventActionArgs eventArgs)
    //{
    //    var random = UnityEngine.Random.Range(0, 3);

    //    switch (random)
    //    {
    //        case 0:
    //            m_player.ChooseRockAttack();
    //            break;
    //        case 1:
    //            m_player.ChoosePaperAttacker();
    //            break;
    //        case 2:
    //            m_player.ChooseScissorAttack();
    //            break;
    //    };
    //}

    //private void OnBattleEnd(object sender, EventActionArgs eventArgs)
    //{
    //    m_handle.StopAllCoroutines();
    //}

    //private void Start()
    //{
    //    m_handle.RoundStart += OnRoundStart;
    //    m_handle.BattleEnd += OnBattleEnd;
    //}
}
