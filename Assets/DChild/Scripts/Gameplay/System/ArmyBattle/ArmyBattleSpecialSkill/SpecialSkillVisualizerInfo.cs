using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class SpecialSkillVisualizerInfo
    {
        [SerializeField]
        private GameObject m_ownerFX;
        [SerializeField]
        private GameObject m_targetFX;

        public GameObject ownerFX => m_ownerFX;
        public GameObject targetFX => m_targetFX;
    }
}

