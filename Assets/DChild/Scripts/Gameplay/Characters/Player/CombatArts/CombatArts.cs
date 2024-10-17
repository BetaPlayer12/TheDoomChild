using DChild.Gameplay.Characters.Player.CombatArt.Leveling;
using DChild.Gameplay.SoulSkills;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class CombatArts : SerializedMonoBehaviour, ISerializable<CombatArtsSaveData>
    {
        [SerializeField]
        private CombatArtLevel m_level;
        [SerializeField]
        private CombatSkillPoints m_skillPoints;
        private int[] m_abilityLevels;

        public CombatArtLevel level => m_level;

        public CombatArtsSaveData SaveData()
        {
            return new CombatArtsSaveData(m_level.Save(), m_skillPoints.points, m_abilityLevels);
        }

        public CombatSkillPoints skillPoints => m_skillPoints;

        public void LoadData(CombatArtsSaveData savedData)
        {
            if (m_abilityLevels == null)
            {
                m_abilityLevels = new int[(int)CombatArt._Count];
            }

            if (savedData != null)
            {
                m_level.Load(savedData.level);

                m_skillPoints.SetPoints(savedData.skillPoints);

                for (int i = 0; i < m_abilityLevels.Length; i++)
                {
                    m_abilityLevels[i] = savedData.GetArtsLevel(i);
                }
            }
            else
            {
                m_level.Load(new CombatArtLevel.SaveData(1, 0));
                m_skillPoints.SetPoints(0);
                for (int i = 0; i < m_abilityLevels.Length; i++)
                {
                    m_abilityLevels[i] = 0;
                }
            }
        }

        public bool IsAbilityActivated(CombatArt battleAbility)
        {
            return GetAbilityLevel(battleAbility) > 0;
        }

        public int GetAbilityLevel(CombatArt battleAbility)
        {
            return m_abilityLevels[(int)battleAbility];
        }

        public void SetAbilityLevel(CombatArt battleAbility, int level)
        {
            m_abilityLevels[(int)battleAbility] = Mathf.Max(0, level);
        }

        public void Initialize()
        {
            if (m_abilityLevels == null)
            {
                m_abilityLevels = new int[(int)CombatArt._Count];
            }

            m_level.Initialize();
        }
    }
}