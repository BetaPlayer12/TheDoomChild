﻿using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public class PlayerCharacterInitializer : MonoBehaviour
    {
        [SerializeField]
        private ComplexCharacterInfo m_info;
        [SerializeField]
        private PlayerSkills m_skills;
        [SerializeField]
        private GameObject m_moduleContainer;

        private void Awake()
        {
            var modules = m_moduleContainer.GetComponentsInChildren<IComplexCharacterModule>(true);
            for (int i = 0; i < modules.Length; i++)
            {
                modules[i].Initialize(m_info);
            }
        }
    }
}