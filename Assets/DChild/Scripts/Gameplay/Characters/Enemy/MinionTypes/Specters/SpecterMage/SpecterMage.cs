using System.Collections;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SpecterMage : Specter
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        private ProjectileInfo m_spellProjectile;
        [SerializeField]
        private Transform m_spellSpawnPoint;

        private ProjectileLauncher m_projectileLauncher;

        protected override SpecterAnimation specterAnimation => null;
        protected override CombatCharacterAnimation animation => null;
        protected override Damage startDamage => m_damage;

        public void ConjurePlasma(ITarget target)
        {
            m_behaviour.StopActiveBehaviour(ref m_waitForBehaviourEnd);
            m_behaviour.SetActiveBehaviour(StartCoroutine(ConjurePlasmaRoutine(target)));
        }

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public override void Turn()
        {
            TurnCharacter();
        }

        public void Idle()
        {
            m_movement.Stop();
        }

        protected override void OnFlinch()
        {

        }

        private IEnumerator ConjurePlasmaRoutine(ITarget target)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            yield return new WaitForSeconds(1f);
            //m_projectileLauncher.FireProjectileTo(m_spellProjectile.projectile, gameObject.scene, m_spellSpawnPoint.position, target.position, m_spellProjectile.speed);
            m_behaviour.SetActiveBehaviour(null);
            m_waitForBehaviourEnd = false;
        }

        protected override void Start()
        {
            base.Start();
           // m_projectileLauncher = new ProjectileLauncher();
        }
    }

}