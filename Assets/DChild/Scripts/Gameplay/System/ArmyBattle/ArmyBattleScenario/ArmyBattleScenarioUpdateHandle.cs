using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleScenarioUpdateHandle : SerializedMonoBehaviour
    {
        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private ArmyBattleScenarioUpdateSubHandle[] m_subhandles;

        List<ArmyBattleScenarioUpdateSubHandle.ScenarioUpdateRequest> m_updateRequests;
        private bool m_doNextRequest;

        public void MoveToNextRequest()
        {
            m_doNextRequest = true;
        }

        public void Initialize(Army player, Army enemy)
        {
            for (int i = 0; i < m_subhandles.Length; i++)
            {
                m_subhandles[i].Initialize(player, enemy);
            }
        }

        public void UpdateScenario(int turnIndex)
        {
            m_updateRequests.Clear();
            for (int i = 0; i < m_subhandles.Length; i++)
            {
                var request = m_subhandles[i].GetValidRequests(turnIndex);
                if (request != null)
                {
                    m_updateRequests.AddRange(request);
                }
            }

            if (m_updateRequests.Count > 0)
            {
                m_updateRequests = m_updateRequests.OrderBy(x => x.priority).ToList();
                StartCoroutine(ApplyRequestRoutines());
            }
            else
            {
                ArmyBattleSystem.StartNewTurn();
            }
        }

        private IEnumerator ApplyRequestRoutines()
        {
            for (int i = m_updateRequests.Count - 1; i >= 0; i--)
            {
                m_updateRequests[i].TriggerScenario();
                m_doNextRequest = false;
                while (m_doNextRequest == false)
                    yield return null;
            }
            ArmyBattleSystem.StartNewTurn();
            m_updateRequests.Clear();
        }
    }
}
