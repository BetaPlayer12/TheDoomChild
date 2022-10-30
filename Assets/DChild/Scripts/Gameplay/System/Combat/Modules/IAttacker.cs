using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IAttacker
    {
        IAttacker parentAttacker { get; }
        IAttacker rootParentAttacker { get; }
        GameObject gameObject { get; }

        event EventAction<CombatConclusionEventArgs> TargetDamaged;

        void SetParentAttacker(IAttacker damageDealer);
        void SetRootParentAttacker(IAttacker damageDealer);

    } 
}