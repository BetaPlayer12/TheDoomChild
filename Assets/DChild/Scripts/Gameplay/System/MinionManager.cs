using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class MinionManager : MonoBehaviour, IGameplaySystemModule, IMinionManager
    {
        [SerializeField]
        private List<ICombatAIBrain> m_registeredMinions = new List<ICombatAIBrain>();
        private bool m_allForbiddenToAttack;
        private bool m_allIgnoreAttack;

        public void Register(ICombatAIBrain minion)
        {
            m_registeredMinions.Add(minion);
            if (m_allForbiddenToAttack)
            {
                minion.ForbidFromAttackTarget(m_allForbiddenToAttack);
            }
            if (m_allIgnoreAttack)
            {
                minion.IgnoreAllTargets(m_allForbiddenToAttack);
            }
        }
        public void Unregister(ICombatAIBrain minion)
        {
            m_registeredMinions.Remove(minion);
        }

        [Button]
        public void ForbidAllFromAttackingTarget(bool notAllowedToAttack)
        {
            //Minions are Volatile so this is done to prevent Dialogue from breaking due to Minions
            try
            {
                for (int i = 0; i < m_registeredMinions.Count; i++)
                {
                    m_registeredMinions[i].ForbidFromAttackTarget(notAllowedToAttack);
                }
                m_allForbiddenToAttack = notAllowedToAttack;
            }
            catch (Exception e)
            {
                m_allForbiddenToAttack = notAllowedToAttack;
                Debug.LogError("Some Minions Are not Behaving Well", this);
                Debug.LogException(e, this);
            }
        }

        [Button]
        public void IgnoreAllTargets(bool willIgnoreTargets)
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                Debug.Log("Enemies Ignoring all Targets");
                m_registeredMinions[i].IgnoreAllTargets(willIgnoreTargets);
            }
            m_allIgnoreAttack = willIgnoreTargets;
        }

        [Button]
        public void IgnoreCurrentTarget()
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                Debug.Log("Enemies Ignoring Current Target");
                m_registeredMinions[i].IgnoreCurrentTarget();
            }
        }

        [Button]
        public void ForcePassiveIdle(bool willForcePassive)
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                m_registeredMinions[i].ForcePassiveIdle(willForcePassive);
            }

        }

        [Button]
        public void IgnorePlayer(bool willIgnoreTargets)
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                Debug.Log("Enemies Ignoring all Targets");
                m_registeredMinions[i].IgnoreCurrentTarget();
                m_registeredMinions[i].IgnoreAllTargets(willIgnoreTargets);
                m_registeredMinions[i].ForbidFromAttackTarget(willIgnoreTargets);
            }
        }
        [Button]
        public void SettoPassive()
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                m_registeredMinions[i].SetPassive();
            }

        }
        [Button]
        public void SettoActive()
        {
            for (int i = 0; i < m_registeredMinions.Count; i++)
            {
                m_registeredMinions[i].SetActive();
            }

        }

        [Button]
        public void showlist()
        {
            Debug.Log(m_registeredMinions.Count);
        }

    }
}
