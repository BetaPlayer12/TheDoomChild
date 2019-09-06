﻿using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Characters.Players;

namespace DChild.Serialization
{
    [System.Serializable, HideLabel, Title("Skills")]
    public struct PrimarySkillsData
    {
        [SerializeField, HideInEditorMode, ReadOnly]
        private bool[] m_movementSkills;

        public PrimarySkillsData(bool[] m_movementSkills) : this()
        {
            this.m_movementSkills = m_movementSkills;
        }

        public bool[] movementSkills { get => m_movementSkills;}
#if UNITY_EDITOR
        [System.Serializable]
        public class Elements : EnumElement<MovementSkill, bool> { }
        [System.Serializable]
        public class ElementList : EnumList<Elements, MovementSkill, bool>
        {

        }

        [Button, HideInPlayMode, PropertyOrder(0)]
        private void Validate()
        {
            m_movementSkills = m_movementSkillList.ToArray();
        }

        [SerializeField, HideInPlayMode, PropertyOrder(1)]
        private ElementList m_movementSkillList;
#endif
    }
}