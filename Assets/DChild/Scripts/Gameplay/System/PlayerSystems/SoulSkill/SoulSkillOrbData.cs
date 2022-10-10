using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [CreateAssetMenu(fileName = "SoulSkillOrbData", menuName = "DChild/Database/Soul Skill Orb")]
    public class SoulSkillOrbData : ScriptableObject
    {
        [SerializeField]
        private Sprite m_availableOrb;
        [SerializeField]
        private Sprite m_activatedOrb;

        public Sprite availableOrb => m_availableOrb;
        public Sprite activatedOrb => m_activatedOrb;
    }
}