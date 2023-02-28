using Holysoft.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAttackSelector : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattleHandle m_battleHandle;
        [SerializeField]
        private PlayerArmyController m_something;
        [SerializeField]
        private ArmyAttackGroupUI m_display;

        private List<ArmyAttackGroup> m_choices;
        private ArmyAttackGroup m_currentChoice;
        private int m_currentChoiceIndex;

        public void Next()
        {
            m_currentChoiceIndex++;
            if (m_currentChoiceIndex >= m_choices.Count)
            {
                m_currentChoiceIndex = 0;
            }
            m_currentChoice = m_choices[m_currentChoiceIndex];
            UpdateDisplay();
        }

        public void Previous()
        {
            m_currentChoiceIndex--;
            if (m_currentChoiceIndex <= -1)
            {
                m_currentChoiceIndex = m_choices.Count - 1;
            }
            m_currentChoice = m_choices[m_currentChoiceIndex];
            UpdateDisplay();
        }

        public void SelectCurrentAttack()
        {
            m_something.ChooseAttack(m_currentChoice);
        }

        public void UpdateChoices(List<ArmyAttackGroup> choices)
        {
            m_choices.Clear();
            m_choices.AddRange(choices);
            m_currentChoiceIndex = 0;
            if (m_choices.Count > 0)
            {
                m_currentChoice = m_choices[m_currentChoiceIndex];
            }
            else
            {
                m_currentChoice = null;
            }
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            m_display.Display(m_currentChoice);
        }

        private void OnRoundStart(object sender, EventActionArgs eventArgs)
        {
            m_currentChoice = null;
            m_currentChoiceIndex = 0;
            UpdateDisplay();
        }

        private void Awake()
        {
            m_battleHandle.RoundStart += OnRoundStart;

            m_choices = new List<ArmyAttackGroup>();
            m_something.AttackTypeChosen += UpdateChoices;
        }


    }
}