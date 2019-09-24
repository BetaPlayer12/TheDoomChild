using DChild.Gameplay.Characters.Enemies;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable]
    public class PhaseInfo<T> where T : System.Enum
    {
        [SerializeField]
        private Dictionary<T, BossPhaseData> m_phaseDataList;
        [SerializeField]
        private IPhaseConditionTemplate<T> m_conditionTemplate;

        public IPhaseInfo GetDataOfPhase(T phase) => m_phaseDataList.ContainsKey(phase) ? m_phaseDataList[phase].info : null;

        public IPhaseConditionHandle<T> CreateConditionHandle(Character character) => m_conditionTemplate.CreateHandle(character);
    }
}