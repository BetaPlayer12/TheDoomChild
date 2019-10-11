/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [DisallowMultipleComponent]
    public class BreakableWall : MonoBehaviour, IDamageable
    {
        [SerializeField, MinValue(1)]
        private int m_hitsToBeDestroyed;
#if UNITY_EDITOR
        [SerializeField, ReadOnly,ProgressBar(0, "m_hitsToBeDestroyed", Segmented = true),HideInEditorMode]
#endif
        private int m_hitsLeft;
        [ShowInInspector, OnValueChanged("UpdateVersion")]
        private bool m_isDestroyed;
        [SerializeField]
        private GameObject m_wall;
        [SerializeField]
        private FX m_damagedFX;
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
                m_hitsLeft -= 1;
                m_damagedFX?.Play();
                DamageTaken?.Invoke(this, new Damageable.DamageEventArgs(1, type));
                if (m_hitsLeft <= 0)
                {
                    if (m_destroyedFX != null)
                    {
                        GameplaySystem.fXManager.InstantiateFX(m_destroyedFX, position);
                    }
                    m_wall?.SetActive(false);
                    Destroyed?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }

        private void UpdateVersion()
        {
            m_wall?.SetActive(!m_isDestroyed);
            m_hitsLeft = m_hitsToBeDestroyed;
        }

        private void Awake()
        {
            m_hitsLeft = m_hitsToBeDestroyed;
        }

        public void SetInvulnerability(bool isInvulnerable)
        {
            throw new System.NotImplementedException();
        }
    }
}