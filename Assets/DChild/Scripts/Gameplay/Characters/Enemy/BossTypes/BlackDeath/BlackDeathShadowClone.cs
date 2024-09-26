using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{

    public class BlackDeathShadowClone : MonoBehaviour
    {
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private BreakableObject m_mainInstance;
        [SerializeField]
        private ProjectileScatterHandle m_attackHandle;
        [SerializeField]
        private float m_attackInterval;
        [SerializeField]
        private float m_afterAppearanceAttackDelay;

        [SerializeField]
        private ParticleSystem m_appearFX;
        [SerializeField]
        private ParticleSystem m_deathFX;
        [SerializeField, BoxGroup("Animations")]
        private SpineRootAnimation m_animation;
        [SerializeField, BoxGroup("Animations"), Spine.Unity.SpineAnimation]
        private string m_idleAnimation;
        [SerializeField, BoxGroup("Animations"), Spine.Unity.SpineAnimation]
        private string m_attackAnimation;

        private float m_attackIntervalTimer;
        private bool m_canAttack;

        public bool isActivated => gameObject.activeInHierarchy;
        

        [Button]
        public void Appear()
        {
            gameObject.SetActive(true);
            m_appearFX.Play();
            m_animation.SetAnimation(0, m_idleAnimation, true);
            m_mainInstance.SetObjectState(false);
            GameplaySystem.combatManager.Heal(m_damageable, 99999999);
            StopAllCoroutines();
            StartCoroutine(AfterAppearanceAttackDelay());
        }

        public void AllowAttack(bool allow)
        {
            m_canAttack = allow;
        }

        private IEnumerator AfterAppearanceAttackDelay()
        {
            AllowAttack(false);
            yield return new WaitForSeconds(m_afterAppearanceAttackDelay);
            m_attackIntervalTimer = 0;
            AllowAttack(true);
        }

        private IEnumerator AttackRoutine()
        {
            AllowAttack(false);
            var attackTrack = m_animation.SetAnimation(0, m_attackAnimation, false);
            m_attackHandle.SpawnProjectiles();
            m_attackHandle.LaunchSpawnedProjectiles();
            yield return new WaitForSpineAnimationComplete(attackTrack);
            m_animation.SetAnimation(0, m_idleAnimation, true);
            m_attackIntervalTimer = m_attackInterval;
            AllowAttack(true);
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            AllowAttack(false);
            m_deathFX.Play();
        }

        private void Awake()
        {
            m_damageable.Destroyed += OnDeath;
        }

        private void Update()
        {
            if (m_canAttack)
            {
                m_attackIntervalTimer -= GameplaySystem.time.deltaTime;
                if (m_attackIntervalTimer < 0)
                {
                    StopAllCoroutines();
                    StartCoroutine(AttackRoutine());

                }
            }
        }
    }

}