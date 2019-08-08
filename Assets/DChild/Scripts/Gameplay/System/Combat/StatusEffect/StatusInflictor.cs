using System;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class StatusInflictor : MonoBehaviour
    {
        [SerializeField]
        private StatusInflictionInfo[] m_statusToInflict;

        private IAttacker m_source;

        public void InflictStatusTo(IStatusReciever reciever) => GameplaySystem.combatManager.InflictStatusTo(reciever, m_statusToInflict);

        private void OnAttackerAttacked(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (DChildUtility.HasInterface<IStatusReciever>(eventArgs.target))
            {
                InflictStatusTo((IStatusReciever)eventArgs.target.instance);
            }
        }

        private void OnEnable()
        {
            m_source = GetComponentInParent<IAttacker>();
            m_source.TargetDamaged += OnAttackerAttacked;
        }

        private void OnDisable()
        {
            m_source.TargetDamaged += OnAttackerAttacked;
        }
    }
}