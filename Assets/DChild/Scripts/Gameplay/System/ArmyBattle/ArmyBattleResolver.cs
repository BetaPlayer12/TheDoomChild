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

            switch (battleResult)
            {
                case Outcome.Win:

                    break;
                case Outcome.Lose:

                    break;
                case Outcome.Draw:

                    break;
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