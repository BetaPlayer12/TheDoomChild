using DChild.Gameplay.Characters.Players.SoulSkills;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class ActivatedSoulSkillListUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_capacityText;

        private SoulSkillUI[] m_buttons;
        private int m_activatedSkillCount;
        private SoulSkillUI GetButton(int index) => m_buttons[index];

        public void ActivateSoulSkill(SoulSkill soulSkill)
        {
            var button = m_buttons[m_activatedSkillCount];
            button.DisplayAs(soulSkill);
            button.Show(false);
            m_activatedSkillCount++;
        }

        public void DeactivateSoulSkill(SoulSkill soulSkill)
        {
            bool deactivatedSkillIsNotLastOfList = false;
            int deactivatedIndex;
            for (deactivatedIndex = m_activatedSkillCount - 1; deactivatedIndex >= 0; deactivatedIndex--)
            {
                var button = m_buttons[deactivatedIndex];
                if (button.soulSkillID == soulSkill.id)
                {
                    m_activatedSkillCount--;
                    if (deactivatedSkillIsNotLastOfList == false)
                    {
                        button.Hide(false);
                        return;
                    }
                    break;
                }
                else
                {
                    if (deactivatedSkillIsNotLastOfList == false)
                    {
                        deactivatedSkillIsNotLastOfList = true;
                    }
                }
            }

            if (deactivatedSkillIsNotLastOfList)
            {
                //Move UI Displayed to Left
                deactivatedIndex++;
                for (; deactivatedIndex <= m_activatedSkillCount; deactivatedIndex++)
                {
                    m_buttons[deactivatedIndex - 1].CopyUI(m_buttons[deactivatedIndex]);
                }
                deactivatedIndex--; //Disregard the last iteration
                var button = m_buttons[deactivatedIndex];
                button.DisplayAs(null);
                button.Hide(false);
            }
        }

        public void SetAsActivedSoulSkills(IReadOnlyCollection<SoulSkill> soulSkill)
        {
            if (soulSkill == null)
            {
                m_activatedSkillCount = 0;
                for (int i = 0; i < m_buttons.Length; i++)
                {
                    m_buttons[i].Hide(true);
                }
            }
            else
            {
                m_activatedSkillCount = soulSkill.Count;
                for (int i = 0; i < m_buttons.Length; i++)
                {
                    if (i < soulSkill.Count)
                    {
                        var button = m_buttons[i];
                        button.DisplayAs(soulSkill.ElementAt(i));
                        button.Show(true);
                    }
                    else
                    {
                        m_buttons[i].Hide(true);
                    }
                }
            }
        }

        public void DisplayCapacity(int capacity)
        {
            m_capacityText.text = capacity.ToString();
        }

        public void Reset()
        {
            if (m_buttons == null)
            {
                m_buttons = GetComponentsInChildren<SoulSkillUI>();
            }

            SetAsActivedSoulSkills(null);
        }
    }
}
