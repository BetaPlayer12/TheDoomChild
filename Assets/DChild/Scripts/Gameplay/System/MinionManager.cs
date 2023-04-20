using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class MinionManager : MonoBehaviour, IGameplaySystemModule, IMinionManager
    {
        [SerializeField]
        private List<ICombatAIBrain> m_registeredMinions = new List<ICombatAIBrain>();
        public void Register(ICombatAIBrain minion)
        {
            m_registeredMinions.Add(minion);
        }
        public void Unregister(ICombatAIBrain minion)
        {

            m_registeredMinions.Remove(minion);
        }

        public void ForbidAllFromAttackingTarget(bool willAttackTarget)
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                Debug.Log("Enemies Forbidden From Attacking");
                m_registeredMinions[i].ForbidFromAttackTarget(willAttackTarget);
            }
        }
        public void IgnoreAllTargets(bool willIgnoreTargets)
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                Debug.Log("Enemies Ignoring all Targets");
                m_registeredMinions[i].IgnoreAllTargets(willIgnoreTargets);
            }
        }

        public void IgnoreCurrentTarget()
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                Debug.Log("Enemies Ignoring Current Target");
                m_registeredMinions[i].IgnoreCurrentTarget();
            }
        }

        public void ForcePassiveIdle(bool willForcePassive)
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                m_registeredMinions[i].ForcePassiveIdle(willForcePassive);
            }

        }

        [Button]
        public void showlist()
        {
            Debug.Log(m_registeredMinions.Count);
        }

    }
}
