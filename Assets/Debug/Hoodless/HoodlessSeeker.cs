using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Gameplay.Characters.Enemies
{
    public class HoodlessSeeker : Minion, IMovingEnemy
    {
        [SerializeField][MinValue(0.1)]
        private float m_speed;
        private PhysicsMovementHandler2D m_movement;

        protected override AttackDamage startDamage => new AttackDamage(AttackType.Physical,1);
        protected override CombatCharacterAnimation animation => null;

        public void MoveTo(Vector2 targetPosition)
        {
            m_movement.MoveTo(targetPosition, m_speed);
        }

        public void Idle()
        {
            m_movement.Stop();
        }

        public void AlertPhase()
        {

        }

        public void PatrolTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_speed);
        }

        public void SafePhase()
        {

        }

        protected override void Start()
        {
            base.Start();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
        }
    }
}
