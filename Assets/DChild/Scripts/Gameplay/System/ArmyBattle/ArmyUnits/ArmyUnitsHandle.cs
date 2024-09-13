using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Units
{
    public abstract class ArmyUnitsHandle : MonoBehaviour
    {
        public abstract void KillOffUnits(int count);
        public abstract void Idle();
        public abstract void Attack();
        public abstract void StopAttack();
        public abstract List<ArmyUnit> GetUnits();
    }
}