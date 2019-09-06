/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Combat;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [DisallowMultipleComponent]
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        [ShowInInspector, OnValueChanged("UpdateVersion")]
        private bool m_isDestroyed;
        [SerializeField]
        private GameObject m_nonDestroyedVersion;
        [SerializeField]
        private GameObject m_destroyedVersion;
        [SerializeField]
        private GameObject m_destroyedFX;

        public Vector2 position => transform.position;

        public bool isAlive => false;
        public IAttackResistance attackResistance => null;

        public event EventAction<Damageable.DamageEventArgs> DamageTaken;
        public event EventAction<EventActionArgs> Destroyed;

        public void SetAs(bool isDestroyed)
        {
            m_isDestroyed = isDestroyed;
            UpdateVersion();
        }

        public void SetHitboxActive(bool enable)
        {
        }

        public void TakeDamage(int totalDamage, AttackType type)
        {
            if (m_isDestroyed == false)
            {
                if (m_destroyedFX != null)
                {
                    GameplaySystem.fXManager.InstantiateFX(m_destroyedFX, position);
                }

                m_nonDestroyedVersion?.SetActive(false);
                m_destroyedVersion?.SetActive(true);
                m_isDestroyed = true;
                DamageTaken?.Invoke(this, new Damageable.DamageEventArgs(1, type));
                Destroyed?.Invoke(this, EventActionArgs.Empty);
            }
        }

        private void UpdateVersion()
        {
            m_nonDestroyedVersion?.SetActive(!m_isDestroyed);
            m_destroyedVersion?.SetActive(m_isDestroyed);
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool m_isInitialized;



        [Button, HideIf("m_isInitialized")]
        private void OnValidate()
        {
            if (m_isInitialized == false)
            {
                m_nonDestroyedVersion = new GameObject("NonDestroyedVersion");
                m_nonDestroyedVersion.transform.parent = transform;
                m_nonDestroyedVersion.transform.localPosition = Vector3.zero;
                m_destroyedVersion = new GameObject("DestroyedVersion");
                m_destroyedVersion.transform.parent = transform;
                m_destroyedVersion.transform.localPosition = Vector3.zero;
                m_destroyedVersion.SetActive(false);
                m_isDestroyed = false;
                m_isInitialized = true;
            }
        }
#endif
    }
}