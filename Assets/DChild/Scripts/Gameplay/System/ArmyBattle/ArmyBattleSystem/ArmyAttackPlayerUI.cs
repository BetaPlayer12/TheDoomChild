using Holysoft.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAttackPlayerUI : MonoBehaviour
    {
        //[SerializeField]
        //private ArmyBattleHandle_TBD m_battleHandle;
        [SerializeField]
        private PlayerArmyController m_source;

        [SerializeField]
        private ArmyAttackOptionsUI m_attackOptions;
        [SerializeField]
        private ArmyAttackSelector m_attackSelector;
        [SerializeField]
        private ArmyAbilityOptionsUI m_abilityOptions;
        [SerializeField]
        private ArmyAbilitySelector m_abilitySelector;

        private void OnPlayerAttackTypeChosen(List<ArmyAttackGroup> obj)
        {
            m_attackSelector.UpdateChoices(obj);
        }

        private void OnRoundStart(object sender, EventActionArgs eventArgs)
        {
            m_attackOptions.UpdateOptions();
            m_attackSelector.Reset();
            m_abilityOptions.UpdateOptions();
            m_abilitySelector.Reset();
        }

        private void OnPlayerAbilityTypeChosen(List<ArmyAbilityGroup> obj)
        {
            m_abilitySelector.UpdateChoices(obj);
        }

        private void Awake()
        {
            //m_battleHandle.RoundStart += OnRoundStart;
            //m_source.AttackTypeChosen += OnPlayerAttackTypeChosen;
            //m_source.AbilityTypeChosen += OnPlayerAbilityTypeChosen;
            //m_attackOptions.Initialize(m_source);
            //m_attackSelector.Initialize(m_source);
            //m_abilityOptions.Initialize(m_source);
            //m_abilitySelector.Initialize(m_source);
        }


    }
}