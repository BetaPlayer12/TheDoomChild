using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "PrimarySkillData", menuName = "DChild/Gameplay/Database/Primary Skill Data")]
    public class PrimarySkillData : ScriptableObject
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private string m_description;
        [SerializeField]
        private PrimarySkill m_skill;
        [SerializeField, PreviewField]
        private Sprite m_border;
        [SerializeField, PreviewField]
        private Sprite m_icon;

        public string skillName => m_name;
        public string description => m_description;
        public PrimarySkill skill => m_skill;
        public Sprite border => m_border;
        public Sprite icon => m_icon;
    }
}
