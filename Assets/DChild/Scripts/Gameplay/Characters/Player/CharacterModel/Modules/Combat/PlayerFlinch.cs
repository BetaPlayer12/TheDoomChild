using System;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using PlayerNew;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFlinch : MonoBehaviour, IFlinch, IComplexCharacterModule
    {
        [SerializeField, MinValue(0)]
        private float m_knockBackPower;
        [SerializeField]
        private CollisionState m_collionState;
        private Rigidbody2D m_physics;
        private Animator m_animator;
        private string m_flinch;

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            m_physics.velocity = Vector2.zero;
            Vector2 knockBackDirection = Vector2.zero;
            knockBackDirection.x = directionToSource.x > 0 ? -1 : 1;

            if (m_collionState.grounded)
            {
                knockBackDirection.y = 1;
            }
            else
            {
                knockBackDirection.y = -directionToSource.y;
            }
            m_physics.AddForce(knockBackDirection * m_knockBackPower, ForceMode2D.Impulse);
            m_animator.SetTrigger(m_flinch);
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_flinch = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Flinch);
            m_physics = info.character.GetComponent<Rigidbody2D>();
            //m_collionState = m_animator.GetComponent<CollisionState>();
           // info.state.canFlinch = true;
        }
    }
}