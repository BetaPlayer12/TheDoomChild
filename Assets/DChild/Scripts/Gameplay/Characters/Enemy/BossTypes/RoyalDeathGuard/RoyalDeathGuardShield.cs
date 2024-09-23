using UnityEngine;
using Sirenix.OdinInspector;
using Holysoft.Event;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardShield : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private Damageable m_damageable;

        [SerializeField]
        private ParticleSystem m_onDamageFX;
        [SerializeField]
        private ParticleSystem m_onDestroyedFX;

        private int m_activationParamID;
        private int m_takeDamageParamID;
        private bool m_isSpawned;

        public event EventAction<EventActionArgs> Destroyed
        {
            add
            {
                m_damageable.Destroyed += value;
            }
            remove
            {
                m_damageable.Destroyed -= value;
            }
        }

        [Button]
        public void Spawn()
        {
            if (m_isSpawned)
                return;

            m_damageable.RessurectSelf();
            m_animator.SetTrigger(m_activationParamID);
            m_animator.ResetTrigger(m_takeDamageParamID);
            m_isSpawned = true;
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_onDestroyedFX.Play();
            m_isSpawned = false;
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_onDamageFX.Play();
            m_animator.SetTrigger(m_takeDamageParamID);
        }

        private void Awake()
        {
            m_activationParamID = Animator.StringToHash("Activate");
            m_takeDamageParamID = Animator.StringToHash("TakeDamage");

            m_damageable.DamageTaken += OnDamageTaken;
            m_damageable.Destroyed += OnDestroyed;
            m_isSpawned = false;
        }
    }
}