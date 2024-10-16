using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills
{
    [System.Serializable]
    public class SpecialSkill : ISpecialSkillImplementor
    {
        [SerializeField]
        private string m_description;
        [SerializeField, Min(1)]
        private int m_duration = 1;
        [SerializeField, HideLabel, HideReferenceObjectPicker, BoxGroup("Visualizers")]
        private SpecialSkillVisualizerInfo m_visualizerInfo = new SpecialSkillVisualizerInfo();
        [SerializeField]
        private ISpecialSkillModule[] m_specialSkillModules = new ISpecialSkillModule[0];

        public int duration => m_duration;
        public SpecialSkillVisualizerInfo visualizerInfo => m_visualizerInfo;
        public string GetDescription() { return m_description; }

        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            for (int i = 0; i < m_specialSkillModules.Length; i++)
            {
                m_specialSkillModules[i].ApplyEffect(owner, target);
            }
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            for (int i = 0; i < m_specialSkillModules.Length; i++)
            {
                m_specialSkillModules[i].RemoveEffect(owner, target);
            }
        }
    }
}

