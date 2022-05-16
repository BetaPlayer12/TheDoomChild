using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Pathfinding;
using DarkTonic.MasterAudio;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/PustuleBomb")]
    public class PustuleBombAI : CombatAIBrain<PustuleBombAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, BoxGroup("Movement")]
            private Vector2 m_forceVelocity;
            public Vector2 forceVelocity => m_forceVelocity;
            [SerializeField, BoxGroup("Movement")]
            private float m_floatTwitchInterval;
            public float floatTwitchInterval => m_floatTwitchInterval;

            [SerializeField, BoxGroup("Attack")]
            private float m_explodeTimer;
            public float explodeTimer => m_explodeTimer;
            [SerializeField, BoxGroup("Attack")]
            private float m_pushForce;
            public float pushForce => m_pushForce;

            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_contactAnimation;
            public string contactAnimation => m_contactAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                //EMPTY
#endif
            }
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_parentObject;
        [SerializeField, TabGroup("Reference")]
        private Transform m_pushDirection;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("HurtBox")]
        private Collider2D m_explodeBB;
        [SerializeField, TabGroup("Chain")]
        private GameObject m_chain;
        [SerializeField, TabGroup("Chain")]
        private List<Rigidbody2D> m_chainRigidbodies;
        
        private bool m_willTwitch;
        private float m_currentTwitchTimer;

        private Coroutine m_floatRoutine;
        private Coroutine m_rotateRoutine;

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
            }
        }

        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //if (m_targetInfo.isValid)
            //{
            //}
            //else
            //{
            //    m_health.SetHealthPercentage(1f);
            //    m_hitbox.Enable();
            //}
            if (m_floatRoutine != null)
            {
                StopCoroutine(m_floatRoutine);
                m_floatRoutine = null;
            }

            m_chain.SetActive(false);
            for (int i = 0; i < m_chainRigidbodies.Count; i++)
            {
                m_chainRigidbodies[i].velocity = Vector2.zero;
            }
            //m_character.physics.SetVelocity(Vector2.zero);
            m_animation.DisableRootMotion();
            m_hitbox.Disable();
            m_animation.SetEmptyAnimation(0, 0);
            StartCoroutine(ExplodeRoutine());
            m_rotateRoutine = StartCoroutine(RotateRoutine());
        }

        private IEnumerator ExplodeRoutine()
        {
            if (m_targetInfo.isValid)
            {
                Vector3 v_diff = (m_targetInfo.position - (Vector2)m_character.centerMass.position);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                m_pushDirection.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                m_character.physics.AddForce(-m_pushDirection.right * m_info.pushForce, ForceMode2D.Force);
                //m_animation.SetAnimation(0, m_info.flinchAnimation, true);
                m_animation.SetAnimation(0, m_info.contactAnimation, true);
                yield return new WaitForSeconds(m_info.explodeTimer);
            }
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForSeconds(1f);
            StopCoroutine(m_rotateRoutine);
            m_character.physics.SetVelocity(Vector2.zero);
            enabled = false;
            m_hitbox.Disable();
            m_bodyCollider.enabled = false;
            m_explodeBB.enabled = true;
            yield return new WaitForSeconds(.25f);
            m_explodeBB.enabled = false;
            m_parentObject.SetActive(false);
            yield return null;
        }

        private IEnumerator RotateRoutine()
        {
            var rotateDirection = UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x);
            while (true)
            {
                this.transform.Rotate(new Vector3(0, 0, 1f), rotateDirection);
                yield return null;
            }
        }

        private IEnumerator FloatingRoutine()
        {
            while (true)
            {
                if (m_willTwitch)
                {
                    m_willTwitch = false;
                    m_character.physics.SetVelocity(Vector2.zero);
                    var forceVelocity = new Vector2(UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x), UnityEngine.Random.Range(-m_info.forceVelocity.y, m_info.forceVelocity.y));
                    m_character.physics.AddForce(forceVelocity, ForceMode2D.Impulse);
                    this.transform.Rotate(new Vector3(0, 0, 1f), UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x));
                }

                if (m_currentTwitchTimer < m_info.floatTwitchInterval)
                {
                    m_currentTwitchTimer += Time.deltaTime;
                }
                else
                {
                    m_willTwitch = true;
                    m_currentTwitchTimer = 0;
                }
                yield return null;
            }
        }

        protected override void Start()
        {
            base.Start();

            m_willTwitch = true;
            m_animation.DisableRootMotion();
            //m_knockbackFlinchHandle.ApplyKnockbackForce(m_info.pushForce);

            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_floatRoutine = StartCoroutine(FloatingRoutine());
        }

        protected override void Awake()
        {
            //Debug.Log(m_info);
            base.Awake();
            //m_hitbox.SetInvulnerability(Invulnerability.Level_2);
        }

        protected override void OnTargetDisappeared()
        {
            m_hitbox.gameObject.SetActive(true);
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            enabled = true;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public override void ReturnToSpawnPoint()
        {
            throw new NotImplementedException();
        }
    }
}