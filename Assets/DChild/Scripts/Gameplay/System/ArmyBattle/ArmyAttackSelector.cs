using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAttackSelector : MonoBehaviour
    {
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
            UpdateDisplay();
        }

        public void Previous()
        {
            m_currentChoiceIndex--;
            if (m_currentChoiceIndex <= -1)
            {
                m_currentChoiceIndex = m_choices.Count - 1;
            }
            UpdateDisplay();
        }

        public void UpdateChoices(List<ArmyAttackGroup> choices)
        {
            m_choices = new List<ArmyAttackGroup>();
            m_currentChoiceIndex = 0;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            m_display.Display(m_choices[m_currentChoiceIndex]);
        }
    }
}