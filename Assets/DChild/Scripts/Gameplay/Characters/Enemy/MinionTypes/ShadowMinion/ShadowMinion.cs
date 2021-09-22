using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ShadowMinion : Minion
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        [MinValue(0f)]
        private float m_chargeSpeed;
        private bool m_inShadowForm;
        private bool m_isCharging;

        private PhysicsMovementHandler2D m_movement;

        public bool inShadowForm => m_inShadowForm;
        public bool isCharging => m_isCharging;

        protected override CombatCharacterAnimation animation => null;
        protected override Damage startDamage => m_damage;

        public void EnterShadowForm()
        {
            m_inShadowForm = true;
            var shadowScale = m_model.localScale;
            shadowScale.y = 0.1f;
            m_model.localScale = shadowScale;
            DisableHitboxes();
            StartCoroutine(FalseWait());
        }

        public void ExitShadowForm()
        {
            m_inShadowForm = false;
            var shadowScale = m_model.localScale;
            shadowScale.y = 1f;
            m_model.localScale = shadowScale;
            EnableHitboxes();
            StartCoroutine(FalseWait());
        }

        public void Idle()
        {

        }

        public void Charge(Vector2 position)
        {
            m_isCharging = true;
            m_movement.MoveTo(position, m_chargeSpeed);
        }

        public void StopCharge()
        {
            m_isCharging = false;
            m_movement.Stop();
            ExitShadowForm();
            StartCoroutine(FalseWait());
        }

        private IEnumerator FalseWait()
        {
            m_waitForBehaviourEnd = true;
            yield return new WaitForSeconds(1f);
            m_waitForBehaviourEnd = false;
        }

        protected override void ResetValues()
        {
            m_inShadowForm = true;
            var shadowScale = m_model.localScale;
            shadowScale.y = 0.1f;
            m_model.localScale = shadowScale;
            DisableHitboxes();
            m_isCharging = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
        }
    }

}