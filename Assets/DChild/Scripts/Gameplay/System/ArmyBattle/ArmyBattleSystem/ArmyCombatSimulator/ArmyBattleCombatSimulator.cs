using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleCombatSimulator
    {
        [SerializeField]
        private ArmyBattleCombatSimulatorData m_data;

        [Button]
        public ArmyBattleCombatResult CalculateCombatResult(ArmyTurnAction player, ArmyTurnAction enemy)
        {
            var basePlayerDamage = GetBaseDamage(player, enemy);
            var baseEnemyDamage = GetBaseDamage(enemy, player);

            var afterCombatPlayerInfo = CalculateCombatResultInfo(player, new ArmyDamage(enemy.attack.type, baseEnemyDamage));
            var afterCombatEnemyInfo = CalculateCombatResultInfo(enemy, new ArmyDamage(player.attack.type, basePlayerDamage));
            var result = new ArmyBattleCombatResult(afterCombatPlayerInfo, afterCombatEnemyInfo);
            Debug.Log(result);
            return result;
        }

        private ArmyBattleCombatResult.Record CalculateCombatResultInfo(ArmyTurnAction target, ArmyDamage attackerDamage)
        {

            var remainingTroopCount = target.troopCount - attackerDamage.value;
            return new ArmyBattleCombatResult.Record(target.troopCount, remainingTroopCount, target.attack.type, attackerDamage.type);
        }

        private int GetBaseDamage(ArmyTurnAction attacker, ArmyTurnAction target)
        {
            var daamgeTypeModifier = m_data.GetDamageTypeModifier(attacker.attack.type, target.attack.type);
            var troopCountModifier = attacker.troopCount / m_data.troopCountConstant;

            var attackValue = attacker.attack.value;
            var randomizedAttackValue = RandomizeAttackValueModifier(attackValue);
            var baseDamage = (attackValue * daamgeTypeModifier * troopCountModifier) + randomizedAttackValue;
            return Mathf.CeilToInt(Mathf.Max(0, baseDamage));
        }

        private int RandomizeAttackValueModifier(float attackValue)
        {
            var maxAttackValueModifier = attackValue * m_data.randomizeAttackValueModifierRange;
            return Mathf.CeilToInt(UnityEngine.Random.Range(-maxAttackValueModifier, maxAttackValueModifier));
        }
    }
}