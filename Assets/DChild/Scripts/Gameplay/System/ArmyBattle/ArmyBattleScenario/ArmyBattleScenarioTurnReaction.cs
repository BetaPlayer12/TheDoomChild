using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleScenarioTurnReaction : ArmyBattleScenarioUpdateSubHandle
    {
        [SerializeField]
        private Dictionary<int, ScenarioUpdateRequest> m_scenarioUpdates;

        public override ScenarioUpdateRequest GetValidRequest(int turnIndex)
        {
            if(m_scenarioUpdates.TryGetValue(turnIndex, out var scenarioUpdateRequest))
            {
                return scenarioUpdateRequest;
            }
            return null;
        }

        public override void Initialize(Army player, Army enemy)
        {
            
        }
    }
}
