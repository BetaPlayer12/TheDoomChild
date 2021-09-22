using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Projectiles;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{

    public class Lich :Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        private GameObject[] m_spells;
        [SerializeField]
        private Transform m_spellSpawnPoint;
        [SerializeField]
        private float m_spellTravelSpeed;


        [SerializeField]
        private float m_moveSpeed;
        private bool m_isHiding;

        private LichAnimation m_animation; 
        private PhysicsMovementHandler2D m_movement;
        private ProjectileLauncher m_spellLauncher;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public bool isHiding => m_isHiding;

        public void Move(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.MoveOnGround(targetPos, m_moveSpeed);
            m_animation.DoMove();
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Detect()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(DetectRoutine()));
        }

        public void Scratch()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(ScratchRoutine()));
        }

        public void ShowSelf()
        {
            m_behaviour.StopActiveBehaviour(ref m_waitForBehaviourEnd);
        }

        public void CastSpell(ITarget target)
        {
            m_behaviour.StopActiveBehaviour(ref m_waitForBehaviourEnd);
            var projectile = m_spells[Random.Range(0, m_spells.Length)];
            m_behaviour.SetActiveBehaviour(StartCoroutine(FireSpellRoutine(projectile, target)));
        }

        public void Turn()
        {
            m_behaviour.StopActiveBehaviour(ref m_waitForBehaviourEnd);
        }

        public void Idle()
        {

        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
        }

        private IEnumerator DetectRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDetect();
            yield return new WaitForAnimationComplete(m_animation.animationState, LichAnimation.ANIMATION_DETECT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ScratchRoutine()
        {
            yield return null;
        }
        private IEnumerator FireSpellRoutine(GameObject spellGO, ITarget target)
        {
            m_waitForBehaviourEnd = true;
            yield return new WaitForSeconds(1f);
            //m_spellLauncher.FireProjectileTo(spellGO, gameObject.scene, m_spellSpawnPoint.position, target.position, m_spellTravelSpeed);
            m_behaviour.SetActiveBehaviour(null);
            m_waitForBehaviourEnd = false;
        }

        protected override void ResetValues()
        {
            m_isHiding = true;
        }

        protected override void Awake()
        {
            base.Awake();
            //m_spellLauncher = new ProjectileLauncher();
        }
    }
}