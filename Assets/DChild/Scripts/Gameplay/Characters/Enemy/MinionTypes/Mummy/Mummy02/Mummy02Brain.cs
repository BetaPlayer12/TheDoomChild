﻿using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Mummy02Brain : MummyBrain<Mummy02>
    {
        [SerializeField]
        private float m_attackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        protected override float attackRange => m_attackRange;
        protected override CountdownTimer attackRest => m_attackRest;

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            else
            {
                m_minion.Stay();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Stay();
            m_attackRest.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
        }

        protected override void Attack()
        {
            m_minion.WhipAttack();
        }
    }
}
