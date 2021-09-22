using System;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BookWorm : Minion
    {
        [SerializeField][MinValue(0f)]
        private float m_patrolSpeed;

        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        private Damage m_devourHeadDPS;
        [SerializeField]
        private GameObject m_acidProjectile;

        private bool m_isAttachedToHead;
        public bool isAttachedToHead => m_isAttachedToHead;

        public override IAttackResistance attackResistance => null;
        protected override CombatCharacterAnimation animation => null;
        protected override Damage startDamage => m_damage;

        public void SpitAcid(ITarget target)
        {

        }

        public void DevourHead(ITarget target)
        {

        }

        public void LetGoOfHead()
        {

        }

        //public void DealDPSTo(IDamageable target) => GameplaySystem.GetRoutine<ICombatRoutine>().ApplyDamage(target, DamageFXType.Pierce, m_devourHeadDPS);

        public void PatrolTo(Vector2 position)
        {

        }

        public void Turn()
        {

        }

        protected override void OnDeath()
        {
            throw new NotImplementedException();
        }

        protected override void ResetValues()
        {
            m_isAttachedToHead = false;
        }
    }

}