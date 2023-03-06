using Holysoft.Event;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAttackPlayerUI : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattleHandle m_battleHandle;
        [SerializeField]
        private PlayerArmyController m_source;

        [SerializeField]
        private ArmyAttackOptionsUI m_options;
        [SerializeField]
        private ArmyAttackSelector m_attackSelector;

        private void OnPlayerAttackTypeChosen(List<ArmyAttackGroup> obj)
        {
            m_attackSelector.UpdateChoices(obj);
        }

        private void OnRoundStart(object sender, EventActionArgs eventArgs)
        {
            m_options.UpdateOptions();
            m_attackSelector.Reset();
        }

        private void Awake()
        {
            m_battleHandle.RoundStart += OnRoundStart;
            m_source.AttackTypeChosen += OnPlayerAttackTypeChosen;
            m_options.Initialize(m_source);
            m_attackSelector.Initialize(m_source);
        }
    }
}