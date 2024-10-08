using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleScenarioUpdateHandle : SerializedMonoBehaviour
    {
        /// <summary>
        /// You  wont need this class when doing Dictionary
        /// </summary>
        [System.Serializable]
        private class ScenarioUpdateRequest
        {
            [SerializeField]
            private DialogueSystemTrigger m_trigger;
            [SerializeField, MinValue(0)]
            private int m_priority;

            public ScenarioUpdateRequest(DialogueSystemTrigger trigger, int priority)
            {
                m_trigger = trigger;
                m_priority = priority;
            }

            public int priority => m_priority;

            public void TriggerScenario()
            {
                m_trigger.OnUse();
            }
        }

        [SerializeField]
        private Dictionary<DialogueSystemTrigger, int> m_dialoguePriorityPairs;


        private List<ScenarioUpdateRequest> m_validRequest;
        private List<DialogueSystemTrigger> m_dialogueTriggers;

        private bool m_moveToNextRequest;

        public void UpdateScenario()
        {
            if(m_dialoguePriorityPairs.Count == 0)
            {
                ArmyBattleSystem.StartNewTurn();
                return;
            }


            GatherValidRequests();

            if (m_validRequest.Count > 0)
            {
                m_validRequest = m_validRequest.OrderBy(x => x.priority).ToList();
                StartCoroutine(ApplyRequestRoutines());
            }
            else
            {
                ArmyBattleSystem.StartNewTurn();
            }
        }

        private void GatherValidRequests()
        {
            if (m_validRequest == null)
            {
                m_validRequest = new List<ScenarioUpdateRequest>();
            }
            m_validRequest.Clear();

            for (int i = 0; i < m_dialogueTriggers.Count; i++)
            {
                var dialogueTrigger = m_dialogueTriggers[i];
                if (dialogueTrigger.condition.IsTrue(null))
                {
                    m_validRequest.Add(new ScenarioUpdateRequest(dialogueTrigger, m_dialoguePriorityPairs[dialogueTrigger]));
                }
            }
        }

        private IEnumerator ApplyRequestRoutines()
        {
            for (int i = 0; i < m_validRequest.Count; i++)
            {
                m_validRequest[i].TriggerScenario();
                m_moveToNextRequest = false;
                while (m_moveToNextRequest == false)
                    yield return null;
            }

            m_validRequest.Clear();
            ArmyBattleSystem.StartNewTurn();
        }

        private void Awake()
        {
            m_validRequest = new List<ScenarioUpdateRequest>();
            m_dialogueTriggers = new List<DialogueSystemTrigger>();

            foreach (var item in m_dialoguePriorityPairs.Keys)
            {
                m_dialogueTriggers.Add(item);
            }
        }
    }
}
