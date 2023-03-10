using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [CreateAssetMenu(fileName = "SoulSkillItem", menuName ="DChild/Database/Soul Skill Item")]
    public class SoulSkillItem : ItemData
    {
        [SerializeField, ToggleGroup("m_enableEdit")]
        private SoulSkill m_soulSkill;

        public SoulSkill soulSkill => m_soulSkill;
    }
}

