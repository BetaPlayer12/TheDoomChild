using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{
    [CreateAssetMenu(fileName = "PlayerSoulSkillsConfiguration", menuName = "DChild/Gameplay/Combat/Player Soul Skills Configuration")]
    public class PlayerSoulSkillsConfiguration : ScriptableObject
    {
        [SerializeField, MinValue(1)]
        private int m_maxActivatedSoulSkill;
        [SerializeField, MinValue(1)]
        private int m_maxSoulCapacity;

        public int maxActivatedSoulSkill => m_maxActivatedSoulSkill;
        public int maxSoulCapacity => m_maxSoulCapacity;
    }
}

