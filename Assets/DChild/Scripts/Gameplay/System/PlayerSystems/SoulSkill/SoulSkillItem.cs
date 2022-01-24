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

    public class SoulSkillItem : ItemData
    {
        #region EDITORONLY

        [SerializeField, ToggleGroup("m_enableEdit")]
        private SoulSkill m_soulSkill;
        [SerializeField, ToggleGroup("m_enableEdit")]
        private Sprite m_icon;
        [SerializeField, TextArea, ToggleGroup("m_enableEdit")]
        private string m_description;
        [NonSerialized, OdinSerialize, ToggleGroup("m_enableEdit")]
        private ISoulSkillModule[] m_modules = new ISoulSkillModule[1];

        public SoulSkill soulSkill => m_soulSkill;
        public Sprite icon => m_icon;
        public string description => m_soulSkill.description;
        #endregion
    }
}

