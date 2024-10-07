using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleScenarioDialogueSystemReaction : ArmyBattleScenarioUpdateSubHandle
    {
        [System.Serializable]
        private class Request : ScenarioUpdateRequest
        {
            [SerializeField]
            private DialogueSystemTrigger m_trigger;

            public bool CanBeTriggered() => m_trigger.condition.IsTrue(null);

            public override void TriggerScenario()
            {
                m_trigger.OnUse();
            }
        }

        [SerializeField]
        private Request[] m_requests;

        public override List<ScenarioUpdateRequest> GetValidRequests(int turnIndex)
        {
            if (m_scenarioUpdateRequests == null)
            {
                m_scenarioUpdateRequests = new List<ScenarioUpdateRequest>();
            }
            m_scenarioUpdateRequests.Clear();

            for (int i = 0; i < m_requests.Length; i++)
            {
                var request = m_requests[i];
                if (request.CanBeTriggered())
                {
                    m_scenarioUpdateRequests.Add(request);
                }
            }

            return m_scenarioUpdateRequests;
        }

        public override void Initialize(Army player, Army enemy)
        {

        }
    }
}
