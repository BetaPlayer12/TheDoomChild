using System.Collections.Generic;
using DChild.Gameplay.Combat;
using UnityEngine;
using Spine.Unity.Modules;
using Sirenix.OdinInspector;
using Holysoft.Event;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class Specter : Minion, ISpawnable, IMovingEnemy, IFlinch
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_moveSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;
        private bool m_isFlickering;

        protected PhysicsMovementHandler2D m_movement;

        public abstract event EventAction<SpawnableEventArgs> Pool;
        public event EventAction<PoolItemEventArgs> PoolRequest;

        protected abstract SpecterAnimation specterAnimation { get; }
        public abstract void SpawnAt(Vector2 position, Quaternion rotation);
        public abstract void ForcePool();
       

        protected abstract void OnFlinch();
        public abstract void Turn();

        public void MoveTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
            if (m_isFlickering)
            {
                specterAnimation?.StopFlicker();
                m_isFlickering = false;
            }
        }

        public void PatrolTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_patrolSpeed);
            specterAnimation?.Flicker();
            m_isFlickering = true;
        }

        public virtual void DestroyItem() => Destroy(gameObject);

        public void SetParent(Transform parent) => transform.parent = parent;

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            if (AttackDamage.IsMagicAttack(damageTypeRecieved))
            {
                m_behaviour.SetActiveBehaviour(null);
                OnFlinch();
            }
            animation.DoDamage();
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
        }
    }
}