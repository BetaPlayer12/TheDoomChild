using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class CombatArts : SerializedMonoBehaviour
    {
        [SerializeField]
        private AetherPoints m_aetherPoints;
        private int[] m_abilityLevels;

        public CombatArtsSaveData SaveData()
        {
            return new CombatArtsSaveData(m_aetherPoints.points, m_abilityLevels);
        }

        public AetherPoints aetherPoints => m_aetherPoints;

        public void LoadData(CombatArtsSaveData savedData)
        {
            if (m_abilityLevels == null)
            {
                m_abilityLevels = new int[(int)CombatArt._Count];
            }

            if (savedData != null)
            {
                m_aetherPoints.SetPoints(savedData.aetherPoints);

                for (int i = 0; i < m_abilityLevels.Length; i++)
                {
                    m_abilityLevels[i] = savedData.GetArtsLevel(i);
                }
            }
            else
            {
                m_aetherPoints.SetPoints(0);
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

        private void Awake()
        {
            if (m_abilityLevels == null)
            {
                m_abilityLevels = new int[(int)CombatArt._Count];
            }
        }
    }
}