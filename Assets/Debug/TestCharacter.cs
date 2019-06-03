using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusInfliction;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class TestCharacter : CombatCharacter, IEnemyTarget
    {
        public override IAttackResistance attackResistance => null;

        public override IStatusEffectState statusEffectState => null;

        public override bool isAlive => throw new System.NotImplementedException();

        public override IStatusResistance statusResistance => throw new System.NotImplementedException();

        public override void DisableController()
        {
            throw new System.NotImplementedException();
        }

        public override void EnableController()
        {
            throw new System.NotImplementedException();
        }

        public override void Heal(int health)
        {
           
        }

        public override void SetFacing(HorizontalDirection facing)
        {
            throw new System.NotImplementedException();
        }

        public override void TakeDamage(int totalDamage, AttackType type)
        {
            Debug.Log("Test Character is Damaged");
        }
    }
}