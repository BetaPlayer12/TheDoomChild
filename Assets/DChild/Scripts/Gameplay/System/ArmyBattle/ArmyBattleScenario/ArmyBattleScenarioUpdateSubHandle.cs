using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public abstract class ArmyBattleScenarioUpdateSubHandle
    {
        [System.Serializable]
        public class ScenarioUpdateRequest
        {
            [SerializeField, MinValue(0)]
            private int m_priority;
            [SerializeField,HideReferenceObjectPicker]
            private UnityEvent m_scenarioModification = new UnityEvent();

            public int priority => m_priority;
            public void TriggerScenario() => m_scenarioModification.Invoke();
        }

        public abstract void Initialize(Army player, Army enemy);
        public abstract ScenarioUpdateRequest GetValidRequest(int turnIndex);
    }
}
