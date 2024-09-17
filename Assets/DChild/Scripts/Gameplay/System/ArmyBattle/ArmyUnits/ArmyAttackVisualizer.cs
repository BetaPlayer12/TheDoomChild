using DChild.Gameplay.ArmyBattle.Battalion;
using DChild.Gameplay.ArmyBattle.Units;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Visualizer
{

    public abstract class ArmyAttackVisualizer : MonoBehaviour
    {
        public abstract void Attack(List<ArmyUnit> units, IArmyBattalion target);
        public abstract void StopAttack(List<ArmyUnit> units);
    }
}