using DChild.Gameplay.Combat;
using Holysoft.Collections;
using Holysoft.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public abstract class DPSObstacle : Obstacle
    {
        [SerializeField]
        private CountdownTimer m_damageInterval;
        private List<ITarget> m_toDamage;

        protected virtual void Damage(ITarget toDamage, AttackInfo info)
        {
            //Dont know how to solve this, there is not enough info on DPSObstacles with bodies
            //var result = GameplaySystem.combatManager.ResolveConflict(info, toDamage);
            //GameplaySystem.combatManager.ResolveConflict(info, toDamage);
        }

        public void Add(ITarget target)
        {
            if (m_toDamage.Contains(target) == false)
            {
                m_toDamage.Add(target);
                if (m_toDamage.Count == 1)
                {
                    this.enabled = true;
                }
            }
        }

        public void Remove(ITarget target)
        {
            if (m_toDamage.Contains(target))
            {
                m_toDamage.Remove(target);
                if (m_toDamage.Count == 0)
                {
                    this.enabled = false;
                    m_damageInterval.Reset();
                }
            }
        }

        private void OnDamageIntervalEnd(object sender, EventActionArgs eventArgs)
        {
            m_damageInterval.Reset();
            var position = transform.position;
            AttackInfo info = new AttackInfo(position, 0, 1, damage);
            for (int i = 0; i < m_toDamage.Count; i++)
            {
                Damage(m_toDamage[i], info);
            }
        }

        private void Awake()
        {
            m_toDamage = new List<ITarget>();
            m_damageInterval.CountdownEnd += OnDamageIntervalEnd;
            enabled = false;
        }

        private void Update()
        {
            m_damageInterval.Tick(GameplaySystem.time.deltaTime);
        }
    }
}

