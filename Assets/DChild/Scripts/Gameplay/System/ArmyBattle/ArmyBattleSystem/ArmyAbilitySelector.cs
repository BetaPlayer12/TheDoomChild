using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAbilitySelector : MonoBehaviour
    {
        [SerializeField]
        private ArmyAbilityGroupUI m_display;

        private PlayerArmyController m_source;
        //private List<ArmyAbilityGroup> m_choices;
        //private ArmyAbilityGroup m_currentChoice;
        private int m_currentChoiceIndex;

        public void Initialize(PlayerArmyController controller)
        {
            m_source = controller;
        }

        public void Next()
        {
            //m_currentChoiceIndex++;
            //if (m_currentChoiceIndex >= m_choices.Count)
            //{
            //    m_currentChoiceIndex = 0;
            //}
            //m_currentChoice = m_choices[m_currentChoiceIndex];
            //UpdateDisplay();
        }

        public void Previous()
        {
            //m_currentChoiceIndex--;
            //if (m_currentChoiceIndex <= -1)
            //{
            //    m_currentChoiceIndex = m_choices.Count - 1;
            //}
            //m_currentChoice = m_choices[m_currentChoiceIndex];
            //UpdateDisplay();
        }

        public void SelectCurrentAbility()
        {
            //m_source.ChooseAbility(m_currentChoice);
        }

        //public void UpdateChoices(List<ArmyAbilityGroup> choices)
        //{
        //    m_choices.Clear();
        //    m_choices.AddRange(choices);
        //    m_currentChoiceIndex = 0;
        //    if (m_choices.Count > 0)
        //    {
        //        m_currentChoice = m_choices[m_currentChoiceIndex];
        //    }
        //    else
        //    {
        //        m_currentChoice = null;
        //    }
        //    UpdateDisplay();
        //}

        //private void UpdateDisplay()
        //{
        //    m_display.Display(m_currentChoice);
        //}

        //private void Awake()
        //{
        //    m_choices = new List<ArmyAbilityGroup>();
        //}

        public void Reset()
        {
           // m_currentChoice = null;
            m_currentChoiceIndex = 0;
            //UpdateDisplay();
        }
    }
}