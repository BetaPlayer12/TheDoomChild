using DChildDebug;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleResolver
    {
        private enum Outcome
        {
            Win,
            Lose,
            Draw
        }


        [SerializeField]
        private ArmyBattleResolverData m_data;

        public void ResolveBattle(ArmyController player, ArmyController enemy)
        {
            var battleResult = GetBattleResult(player.currentAttack.type, enemy.currentAttack.type);
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, $"Player's <{player.currentAttack.type.ToString()}> vs Enemy's <{enemy.currentAttack.type.ToString()}>:: Result: <{battleResult.ToString()}>");

            var playerDamage = 0;
            var enemyDamage = 0;

            switch (battleResult)
            {
                case Outcome.Win:
                    playerDamage = CalculateDamageDealt(player.currentAttack.value, Outcome.Win);
                    enemyDamage = CalculateDamageDealt(enemy.currentAttack.value, Outcome.Lose);
                    break;
                case Outcome.Lose:
                    playerDamage = CalculateDamageDealt(player.currentAttack.value, Outcome.Lose);
                    enemyDamage = CalculateDamageDealt(enemy.currentAttack.value, Outcome.Win);
                    break;
                case Outcome.Draw:
                    playerDamage = CalculateDamageDealt(player.currentAttack.value, Outcome.Draw);
                    enemyDamage = CalculateDamageDealt(enemy.currentAttack.value, Outcome.Draw);
                    break;
            }

            enemy.controlledArmy.troopCount.ReduceCurrentValue(playerDamage);
            player.controlledArmy.troopCount.ReduceCurrentValue(enemyDamage);
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, $"\n Player Dealt: {playerDamage} \n Enemy Dealt: {enemyDamage}");
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, $"\n Player Troops: {player.controlledArmy.troopCount.currentValue} \n Enemy Troops: {enemy.controlledArmy.troopCount.currentValue}");
        }

        private int CalculateDamageDealt(int power, Outcome battleResult) => Mathf.FloorToInt(power * GetOutcomeDamageModifier(battleResult));

        private float GetOutcomeDamageModifier(Outcome battleResult)
        {
            switch (battleResult)
            {
                case Outcome.Win:
                    return m_data.winModifier;
                case Outcome.Lose:
                    return m_data.loseModifier;
                case Outcome.Draw:
                    return m_data.drawModifier;
                default:
                    return 1f;
            }
        }

        private Outcome GetBattleResult(UnitType player, UnitType enemy)
        {
            if (player == enemy)
                return Outcome.Draw;

            switch (player, enemy)
            {
                case (UnitType.Paper, UnitType.Rock):
                case (UnitType.Rock, UnitType.Scissors):
                case (UnitType.Scissors, UnitType.Paper):
                    return Outcome.Win;

                default:
                    return Outcome.Lose;
            }
        }
    }
}