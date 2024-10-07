using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public abstract class ArmyBattleScenarioUpdateSubHandle
    {
        [System.Serializable]
        public abstract class ScenarioUpdateRequest
        {
            [SerializeField, MinValue(0)]
            private int m_priority;

            public int priority => m_priority;
            public abstract void TriggerScenario();
        }

        protected List<ScenarioUpdateRequest> m_scenarioUpdateRequests = new List<ScenarioUpdateRequest>();

        public abstract void Initialize(Army player, Army enemy);
        public abstract List<ScenarioUpdateRequest> GetValidRequests(int turnIndex);
    }
}
