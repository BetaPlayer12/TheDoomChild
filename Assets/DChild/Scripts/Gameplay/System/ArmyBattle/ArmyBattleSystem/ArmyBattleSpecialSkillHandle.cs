using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace DChild.Gameplay.ArmyBattle
{

    public class ArmyBattleSpecialSkillHandle : MonoBehaviour
    {
        [System.Serializable]
        private class ActiveSkill
        {
            [SerializeField]
            private SpecialSkill m_specialSkill;
            [SerializeField]
            private ArmyController m_owner;
            public int turnsLeft;

            public ActiveSkill(SpecialSkill specialSkill, ArmyController owner)
            {
                m_specialSkill = specialSkill;
                m_owner = owner;
                turnsLeft = specialSkill.duration;
            }

            public SpecialSkill specialSkill => m_specialSkill;
            public ArmyController owner => m_owner;
        }

        [SerializeField]
        private List<ActiveSkill> m_activeSkillList;

        public void Activate(SpecialSkill specialSkill, ArmyController owner)
        {
            if (specialSkill != null && owner != null)
            {
                var skill = new ActiveSkill(specialSkill, owner);
                //This creates a Circle Dependency
                skill.specialSkill.ApplyEffect(owner, ArmyBattleSystem.GetTargetOf(owner));
                m_activeSkillList.Add(skill);
            }
        }

        public void ResolveActiveSkills()
        {
            for (int i = m_activeSkillList.Count - 1; i >= 0; i--)
            {
                var currentSkill = m_activeSkillList[i];
                currentSkill.turnsLeft -= 1;
                if (currentSkill.turnsLeft == 0)
                {
                    var owner = currentSkill.owner;
                    currentSkill.specialSkill.RemoveEffect(owner, ArmyBattleSystem.GetTargetOf(owner));
                    m_activeSkillList.RemoveAt(i);
                }
            }
        }
    }
}