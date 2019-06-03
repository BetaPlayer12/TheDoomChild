using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class FlyingEye : Minion, IMovingEnemy, IFlinch
    {
        [SerializeField]
        protected AttackDamage m_damage;
        [SerializeField]
        [MinValue(0f)]
        protected float m_moveSpeed;

        protected PhysicsMovementHandler2D m_movement;
        protected override AttackDamage startDamage => m_damage;

        public abstract void MoveTo(Vector2 destination);
        public abstract void Turn();
        protected abstract IEnumerator FlinchRoutine();

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
        }
    }

}