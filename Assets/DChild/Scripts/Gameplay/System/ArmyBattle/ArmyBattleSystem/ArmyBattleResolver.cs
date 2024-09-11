using DChildDebug;
using NUnit.Framework.Constraints;
using UnityEngine;
using System;

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

        public void ResolveBattle(IArmyCombatInfo player, IArmyCombatInfo enemy)
        {
            var battleResult = GetBattleResult(player.attackInfo.type, enemy.attackInfo.type);
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, $"Player's <{player.attackInfo.type.ToString()}> vs Enemy's <{enemy.attackInfo.type.ToString()}>:: Result: <{battleResult.ToString()}>");

            var initialPlayerDamage = player.attackInfo.value;
            var initialEnemyDamage = enemy.attackInfo.value;

            var playerDamage = 0;
            var enemyDamage = 0;

            switch (battleResult)
            {
                case Outcome.Win:
                    playerDamage = CalculateDamageDealt(initialPlayerDamage, Outcome.Win);
                    enemyDamage = CalculateDamageDealt(initialEnemyDamage, Outcome.Lose);
                    break;
                case Outcome.Lose:
                    playerDamage = CalculateDamageDealt(initialPlayerDamage, Outcome.Lose);
                    enemyDamage = CalculateDamageDealt(initialEnemyDamage, Outcome.Win);

                    //if (Random.Range(0f, 1f) <= m_data.chanceToLoseACharacter)
                    //{
                    //    var removedCharacter = player.controlledArmy.RemoveRandomCharacter(player.currentAttack.type);
                    //    CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, $"\n Player Lost {removedCharacter.name}");
                    //}
                    break;
                case Outcome.Draw:
                    playerDamage = CalculateDamageDealt(initialPlayerDamage, Outcome.Draw);
                    enemyDamage = CalculateDamageDealt(initialEnemyDamage, Outcome.Draw);
                    break;
            }

            playerDamage -= CalculateDamageReduction(playerDamage, player.attackInfo.type, enemy.damageReductionModifier);
            enemy.troopCount.ReduceCurrentValue(playerDamage);

            enemyDamage -= CalculateDamageReduction(enemyDamage, enemy.attackInfo.type, player.damageReductionModifier);
            player.troopCount.ReduceCurrentValue(enemyDamage);

            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, $"\n Player Dealt: {playerDamage} \n Enemy Dealt: {enemyDamage}");
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, $"\n Player Troops: {player.troopCount.currentValue} \n Enemy Troops: {enemy.troopCount.currentValue}");
        }

        private int CalculateDamageDealt(int power, Outcome battleResult) => m_data.baseDamageValue + Mathf.FloorToInt(power * GetOutcomeDamageModifier(battleResult));

        private int CalculateDamageReduction(int damage, UnitType type, ArmyDamageTypeModifier modifier)
        {
            //return Mathf.CeilToInt(modifier.GetModifier(type) * damage);
            throw new NotImplementedException();
            return 0;
        }

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