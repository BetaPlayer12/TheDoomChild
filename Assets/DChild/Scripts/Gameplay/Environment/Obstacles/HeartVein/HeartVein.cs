using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment.Platforms;

namespace DChild.Gameplay.Environment
{
    public class HeartVein : DestructableObject
    {
        //        [SerializeField]
        //        private BasicHealth m_health;
        //        [SerializeField]
        //        private BasicAttackResistance m_resistance;
        //        [SerializeField]
        //        private RepositionPlatform m_platform;
        //        private HeartVeinAnimation m_animation;

        //        private bool m_hasBurst;

        //        public override IAttackResistance attackResistance => m_resistance;

        //        public override Vector2 position => transform.position;

        //        public void Load(Serializer.Data data)
        //        {
        //            m_hasBurst = data.hasBurst;
        //            if (m_hasBurst)
        //            {
        //                m_platform.SnapTo(1);
        //                m_animation.StartAsBurstAnimation();
        //                DisableHitboxes();
        //            }
        //            else
        //            {
        //                m_platform.SnapTo(0);
        //                m_health.rese();
        //                m_animation.DoIdle();
        //                EnableHitboxes();
        //            }

        //            StopAllCoroutines();
        //        }

        //        public Serializer.Data Save() => new Serializer.Data(m_hasBurst);

        //        public override AttackDamage[] TakeExternalDamage(DamageSource source, params AttackDamage[] damages)
        //        {
        //            int totalDamageRecieved;
        //            var damageReport = ApplyDamage(damages, out totalDamageRecieved);
        //            if (m_hasBurst == false)
        //            {
        //                m_animation.DoBump();
        //            }

        //            return damageReport;
        //        }

        //        public override AttackDamage[] TakeInternalDamage(params AttackDamage[] damages)
        //        {
        //            return null;
        //        }

        //        protected EventActionArgs OnDeath(object sender, EventActionArgs eventArgs)
        //        {
        //            m_hasBurst = true;
        //            StartCoroutine(BurstRoutine());
        //            DisableHitboxes();
        //            return eventArgs;
        //        }

        //        private IEnumerator BurstRoutine()
        //        {
        //            m_animation.DoBurst();
        //            yield return new WaitForAnimationEvent(m_animation.animationState, HeartVeinAnimation.EVENT_BURST);
        //            m_platform.MoveTo(1);
        //        }

        //        protected override void Awake()
        //        {
        //            base.Awake();
        //            m_hasBurst = false;
        //            m_health.InitializeHealth();
        //            m_animation = GetComponentInChildren<HeartVeinAnimation>();
        //            m_health.Death += OnDeath;
        //        }

        //#if UNITY_EDITOR
        //        [Button("Burst")]
        //        private void Burst()
        //        {
        //            OnDeath(this, EventActionArgs.Empty);
        //        }
        //#endif
        public override Vector2 position
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public override IAttackResistance attackResistance => null;

        public override void Heal(int health)
        {
            throw new System.NotImplementedException();
        }

        public override void TakeDamage(int totalDamage, AttackType type)
        {
            throw new System.NotImplementedException();
        }
    }
}

