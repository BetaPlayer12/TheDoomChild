using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Items;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [CreateAssetMenu(fileName = "SoulCrystal", menuName = "DChild/Database/Soul Crystal")]
    public class SoulCrystal : ItemData
    {
        [SerializeField, OnValueChanged("UpdateData"), ToggleGroup("m_enableEdit")]
        private SoulSkill m_activateSoulSkill;
        
        [SerializeField, ToggleGroup("m_enableEdit")]
        private IChallenge[] m_challenges;

        public string crystalName => m_name;
        public string challengeInfo
        {
            get
            {
                string message = "";
                for (int i = 0; i < m_challenges.Length; i++)
                {
                    message += m_challenges[i].message;
                }
                return message;
            }
        }

        public bool IsQualifiedToActiveSkill()
        {
            for (int i = 0; i < m_challenges.Length; i++)
            {
                if (m_challenges[i].IsComplete() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public IChallenge[] CreateChallenges()
        {
            var challenges = new IChallenge[m_challenges.Length];
            for (int i = 0; i < m_challenges.Length; i++)
            {
                m_challenges[i] = challenges[i].CreateCopy();
            }
            return challenges;
        }

#if UNITY_EDITOR
        protected override string fileSuffix => "CrystalData";

        private void UpdateData()
        {
            m_name = m_activateSoulSkill.name;
            m_customName = m_name;
            UpdateReference();
            m_name += " Crystal";
        }
#endif

        public string name => m_name;

    }
}