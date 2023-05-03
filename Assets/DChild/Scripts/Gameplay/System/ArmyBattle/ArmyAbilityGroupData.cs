﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyAbilityGroupData", menuName = "DChild/Gameplay/Army/Ability Group")]
    public class ArmyAbilityGroupData : SerializedScriptableObject
    {
        [SerializeField]
        private string m_name;
        [SerializeField, TextArea]
        private string m_description;
        [SerializeField]
        private bool m_useCharactersForUseCount;
        [SerializeField]
        private ArmyCharacter[] m_members;
        [SerializeField]
        private IArmyAbilityEffect[] m_effects = new IArmyAbilityEffect[0];

        public string groupName => m_name;
        public string description => m_description;
        public bool useCharactersForUseCount => m_useCharactersForUseCount;
        public int memberCount => m_members.Length;
        public ArmyCharacter GetMember(int index) => m_members[index];

        public void ApplyEffect(Army owner, Army opponent)
        {
            for (int i = 0; i < m_effects.Length; i++)
            {
                m_effects[i].ApplyEffect(owner, opponent);
            }
        }
    }
}