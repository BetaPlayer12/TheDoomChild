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
            [SerializeField, BoxGroup("Movement")]
            private float m_idleTimeScale;
            public float idleTimeScale => m_idleTimeScale;
            [SerializeField, BoxGroup("Movement")]
            private float m_twitchTimeScale;
            public float twitchTimeScale => m_twitchTimeScale;
            [SerializeField, BoxGroup("Movement")]
            private float m_chainsDrag;
            public float chainsDrag => m_chainsDrag;

            [SerializeField, BoxGroup("Attack")]
            private float m_explodeTimer;
            public float explodeTimer => m_explodeTimer;
            [SerializeField, BoxGroup("Attack")]
            private float m_pushForce;
            public float pushForce => m_pushForce;

            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idle1Animation;
            public string idle1Animation => m_idle1Animation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idle3Animation;
            public string idle3Animation => m_idle3Animation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_twitch1Animation;
            public string twitch1Animation => m_twitch1Animation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_twitch2Animation;
            public string twitch2Animation => m_twitch2Animation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_twitch3Animation;
            public string twitch3Animation => m_twitch3Animation;
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
        private GameObject m_model;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_parentObject;
        [SerializeField, TabGroup("Reference")]
        private Transform m_pushDirection;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private DamageContactLocator m_damageContactLocator;
        [SerializeField, TabGroup("HurtBox")]
        private Collider2D m_explodeBB;
        [SerializeField, TabGroup("Chain")]
        private GameObject m_chain;
        [SerializeField, TabGroup("Chain")]
        private List<Rigidbody2D> m_chainRigidbodies;

        private string m_currentIdleAnimation;
        private string m_currentTwitchAnimation;
        
        private bool m_willTwitch;
        private float m_currentTwitchTimer;

        private Coroutine m_floatRoutine;

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null && !m_health.isEmpty)
            {
                base.SetTarget(damageable);
                Debug.Log("PUSTULE DETECTED PLAYER");
                Death();
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
            //m_rotateRoutine = StartCoroutine(RotateRoutine());
            Death();
        }

        private void Death()
        {
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
        }

        private IEnumerator ExplodeRoutine()
        {
            enabled = false;
            if (m_health.isEmpty)
            {
                yield return new WaitUntil(() => m_damageContactLocator.damageContactPoint != Vector2.zero);
                Vector3 v_diff = (m_damageContactLocator.damageContactPoint - (Vector2)m_character.centerMass.position);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                m_pushDirection.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                m_character.physics.AddForce(-m_pushDirection.right * m_info.pushForce, ForceMode2D.Force);
            }
            //m_animation.SetAnimation(0, m_info.flinchAnimation, true);
            m_animation.SetAnimation(0, m_info.contactAnimation, true);
            yield return new WaitForSeconds(m_info.explodeTimer);

            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForSeconds(1f);
            //StopCoroutine(m_rotateRoutine);
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

        //private IEnumerator RotateRoutine()
        //{
        //    var rotateDirection = UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x);
        //    while (true)
        //    {
        //        m_model.transform.Rotate(new Vector3(0, 0, 1f), rotateDirection);
        //        yield return null;
        //    }
        //}

        private IEnumerator FloatingRoutine()
        {
            while (true)
            {
                if (m_willTwitch)
                {
                    m_willTwitch = false;
                    //m_character.physics.SetVelocity(Vector2.zero);
                    //var forceVelocity = new Vector2(UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x), UnityEngine.Random.Range(-m_info.forceVelocity.y, m_info.forceVelocity.y));
                    //m_character.physics.AddForce(forceVelocity, ForceMode2D.Impulse);
                    //m_model.transform.Rotate(new Vector3(0, 0, 1f), UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x));

                    m_animation.SetAnimation(1, m_currentTwitchAnimation, false).TimeScale = m_info.twitchTimeScale;
                    m_animation.AddEmptyAnimation(1, 0, 0);
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

            for (int i = 0; i < m_chainRigidbodies.Count; i++)
            {
                m_chainRigidbodies[i].drag = m_info.chainsDrag;
            }

            var randomIdlePicker = UnityEngine.Random.Range(0, 3);
            switch (randomIdlePicker)
            {
                case 0:
                    m_currentIdleAnimation = m_info.idle1Animation;
                    m_currentTwitchAnimation = m_info.twitch1Animation;
                    break;
                case 1:
                    m_currentIdleAnimation = m_info.idle2Animation;
                    m_currentTwitchAnimation = m_info.twitch2Animation;
                    break;
                case 2:
                    m_currentIdleAnimation = m_info.idle3Animation;
                    m_currentTwitchAnimation = m_info.twitch3Animation;
                    break;
            }
            m_animation.SetAnimation(0, m_currentIdleAnimation, true).TimeScale = m_info.idleTimeScale;
            m_floatRoutine = StartCoroutine(FloatingRoutine());
        }

        protected override void Awake()
        {
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