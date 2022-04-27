using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public abstract class AttackBehaviour : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule, IResettableBehaviour
    {
        [System.Serializable]
        protected class Info
        {
            [SerializeField]
            private ParticleSystem m_fx;
            [SerializeField]
            private Transform m_fxPosition;
            [SerializeField]
            private Collider2D m_collider;
            [SerializeField, MinValue(0)]
            private float m_damageModifier = 1;
            [SerializeField, MinValue(0)]
            private float m_nextAttackDelay; 

            public float nextAttackDelay => m_nextAttackDelay;
            public float damageModifier => m_damageModifier;
            public Transform fxPosition => m_fxPosition;

            public void PlayFX(bool value)
            {
                if (m_fx != null)
                {
                    if (value)
                    {
                        m_fx.Play(true);
                    }
                    else
                    {
                        m_fx.Stop(true);
                    } 
                }
            }

            public void ClearFX() => m_fx?.Clear();

            public void ShowCollider(bool value)
            {
                m_collider.enabled = value;
            }
        }

        protected float m_timer;
        protected IAttackState m_state;
        protected Animator m_animator;
        protected int m_animationParameter;
        protected Rigidbody2D m_rigidBody;
        protected Attacker m_attacker;

        public virtual void Cancel()
        {
            if (m_state.isAttacking)
            {
                m_animator.SetBool(m_animationParameter, false);
                m_state.isAttacking = false;
                m_state.waitForBehaviour = false;
                m_state.canAttack = true;
            }
        }

        public virtual void AttackOver()
        {
            m_animator.SetBool(m_animationParameter, false);
            m_state.isAttacking = false;
            m_state.waitForBehaviour = false;
        }

        public virtual void Initialize(ComplexCharacterInfo info)
        {
            m_rigidBody = info.rigidbody;
            m_attacker = info.attacker;
            m_state = info.state;
            m_state.canAttack = true;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsAttacking);
        }

        public virtual void Reset()
        {
            m_timer = 0;
            m_state.canAttack = true;
        }
    }
}
