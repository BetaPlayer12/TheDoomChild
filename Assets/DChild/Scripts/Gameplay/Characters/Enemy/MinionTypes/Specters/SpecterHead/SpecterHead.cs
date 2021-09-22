using System.Collections;
using System.Collections.Generic;
using Holysoft;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SpecterHead : Specter
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        [MinValue(0f)]
        private float m_dashSpeed;

        public float dashSpeed => m_dashSpeed;
        protected override SpecterAnimation specterAnimation => null;
        protected override CombatCharacterAnimation animation => null;
        protected override Damage startDamage => m_damage;

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void Dash(ITarget target, float duration)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DashRoutine(target, duration)));
        }

        public override void Turn()
        {
            TurnCharacter();
        }

        protected override void OnFlinch()
        {

        }

        private IEnumerator DashRoutine(ITarget target, float duration)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            yield return new WaitForSeconds(1f);
            m_movement.MoveTo(target.position, m_dashSpeed);
            yield return new WaitForSeconds(duration);
            m_movement.Stop();
            StopActiveBehaviour();
        }

    }

}