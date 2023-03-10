using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [CreateAssetMenu(fileName = "SoulSkillOrbData", menuName = "DChild/Database/Soul Skill Orb")]
    public class SoulSkillOrbData : ScriptableObject
    {
        [System.Serializable]
        public class OrbInfo
        {
            [SerializeField]
            private Sprite m_orbSprite;
            [SerializeField]
            private Material m_orbMaterial;
            [SerializeField]
            private Material m_glowMaterial;

            public Sprite orbSprite => m_orbSprite;
            public Material orbMaterial => m_orbMaterial;
            public Material glowMaterial => m_glowMaterial;
        }

        [SerializeField, BoxGroup("Available Orb"), HideLabel]
        private OrbInfo m_availableOrb;
        [SerializeField, BoxGroup("Activated Orb"), HideLabel]
        private OrbInfo m_activatedOrb;

        public OrbInfo availableOrb => m_availableOrb;
        public OrbInfo activatedOrb => m_activatedOrb;
    }
}