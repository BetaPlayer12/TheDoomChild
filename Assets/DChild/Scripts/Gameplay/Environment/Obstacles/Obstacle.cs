using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public abstract class Obstacle : MonoBehaviour, IAttacker, IDamageDealer
    {
        protected abstract AttackDamage damage { get; }
        public event EventAction<CombatConclusionEventArgs> TargetDamaged;

        public virtual void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
        {
            if (!targetDefense.isInvulnerable)
            {
                var position = transform.position;
                AttackInfo info = new AttackInfo(position, 0, 1, damage);
                var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                TargetDamaged?.Invoke(this, new CombatConclusionEventArgs(info, targetInfo.target, result));
            }
        }
    }
}