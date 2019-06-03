using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Mummy01Brain : MummyBrain<Mummy01>
    {
        [SerializeField]
        private float m_attackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        protected override float attackRange => m_attackRange;
        protected override CountdownTimer attackRest => m_attackRest;

        protected override void Attack()
        { 
            var rand = Mathf.Abs(Random.Range(1, 3));

            switch (rand)
            {
                case 1:
                    m_minion.WhipAttack();
                    break;
                case 2:
                    m_minion.StabAttack();
                    break;
            }
        }

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
    }
}
