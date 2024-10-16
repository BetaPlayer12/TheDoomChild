using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills
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
            private ISpecialSkillVisuals m_ownerVisuals;
            private ISpecialSkillVisuals m_targetVisuals;


            public ActiveSkill(SpecialSkill specialSkill, ArmyController owner, ArmyController target)
            {
                m_specialSkill = specialSkill;
                m_owner = owner;
                turnsLeft = specialSkill.duration;

                var visualizerInfo = specialSkill.visualizerInfo;
                if (visualizerInfo.ownerFX != null)
                {
                    m_ownerVisuals = Object.Instantiate(visualizerInfo.ownerFX).GetComponent<ISpecialSkillVisuals>();
                    m_ownerVisuals.transform.position = ArmyBattleSystem.GetBattlationPositionOf(owner);

                }

                if (visualizerInfo.targetFX != null)
                {
                    m_targetVisuals = Object.Instantiate(visualizerInfo.targetFX).GetComponent<ISpecialSkillVisuals>();
                    m_targetVisuals.transform.position = ArmyBattleSystem.GetBattlationPositionOf(target);
                }
            }

            private int activationTurnCount => turnsLeft - m_specialSkill.duration;

            public SpecialSkill specialSkill => m_specialSkill;
            public ArmyController owner => m_owner;

            public void PlayVisuals()
            {
                m_ownerVisuals?.Play(activationTurnCount);
                m_targetVisuals?.Play(activationTurnCount);
            }

            public void DestroyVisuals()
            {
                Destroy(m_ownerVisuals.gameObject);
                Destroy(m_targetVisuals.gameObject);
            }
        }

        [SerializeField]
        private List<ActiveSkill> m_activeSkillList;

        public void Activate(SpecialSkill specialSkill, ArmyController owner)
        {
            if (specialSkill != null && owner != null)
            {
                var target = ArmyBattleSystem.GetTargetOf(owner);
                var skill = new ActiveSkill(specialSkill, owner, target);
                //This creates a Circle Dependency
                skill.specialSkill.ApplyEffect(owner, target);
                m_activeSkillList.Add(skill);
            }
        }

        public void ReinstanteActivateEffects()
        {
            for (int i = 0; i < m_activeSkillList.Count; i++)
            {
                var currentSkill = m_activeSkillList[i];
                currentSkill.PlayVisuals();
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
                    currentSkill.DestroyVisuals();
                    m_activeSkillList.RemoveAt(i);
                }
            }
        }
    }
}