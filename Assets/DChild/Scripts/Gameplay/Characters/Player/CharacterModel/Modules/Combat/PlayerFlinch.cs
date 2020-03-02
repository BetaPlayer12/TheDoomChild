using System;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFlinch : MonoBehaviour, IFlinch, IComplexCharacterModule
    {
        [SerializeField, MinValue(0)]
        private float m_knockBackPower;

        private CharacterPhysics2D m_physics;
        private Animator m_animator;
        private string m_flinch;

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            m_physics.SetVelocity(0);
            Vector2 knockBackDirection = Vector2.zero;
            knockBackDirection.x = directionToSource.x > 0 ? -1 : 1;

            if (m_physics.onWalkableGround)
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
            m_physics = info.physics;
            info.state.canFlinch = true;
        }
    }
}