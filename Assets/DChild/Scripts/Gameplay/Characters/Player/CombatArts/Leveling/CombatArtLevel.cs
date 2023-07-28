using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Player.CombatArt.Leveling
{
    [System.Serializable]
    public class CombatArtLevel : MonoBehaviour
    {
        public struct SaveData
        {
            public SaveData(int level, int exp)
            {
                this.level = level;
                this.exp = exp;
            }

            public int level { get; }
            public int exp { get; }
        }

        [SerializeField, MinValue(1)]
        private int m_currentLevel = 1;
        [SerializeField]
        private CombatArtExperiencePoint m_exp;

        public event EventAction<EventActionArgs> OnLevelUp;

        public ICappedStat exp => m_exp;

        public int currentLevel => m_currentLevel;

        public SaveData Save()
        {
            return new SaveData(m_currentLevel, m_exp.currentValue);
        }

        public void Load(SaveData data)
        {
            m_currentLevel = data.level;
            m_exp.SetCurrentValue(data.exp);
        }

        public void Initialize()
        {
            m_exp.MaxValueReached += OnMaxValueReached;
        }

        private void OnMaxValueReached(object sender, EventActionArgs eventArgs)
        {
            m_currentLevel++;
            OnLevelUp?.Invoke(this, EventActionArgs.Empty);
        }
    }
}