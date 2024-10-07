﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleScenarioTurnReaction : ArmyBattleScenarioUpdateSubHandle
    {
        [System.Serializable]
        private class Request : ScenarioUpdateRequest
        {
            [SerializeField]
            private UnityEvent m_scenarioModification;

            public override void TriggerScenario()
            {
                m_scenarioModification?.Invoke();
            }
        }

        [SerializeField]
        private Dictionary<int, ScenarioUpdateRequest> m_scenarioUpdates;

        public override List<ScenarioUpdateRequest> GetValidRequests(int turnIndex)
        {
            if (m_scenarioUpdateRequests == null)
            {
                m_scenarioUpdateRequests = new List<ScenarioUpdateRequest>();
            }
            m_scenarioUpdateRequests.Clear();

            if (m_scenarioUpdates.TryGetValue(turnIndex, out var scenarioUpdateRequest))
            {
                m_scenarioUpdateRequests.Add(scenarioUpdateRequest);
            }
            return m_scenarioUpdateRequests;
        }

        public override void Initialize(Army player, Army enemy)
        {

        }
    }
}
