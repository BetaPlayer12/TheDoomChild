using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    public class ArmyBattleScenarioTest : ArmyBattleScenarioHandle
    {
        public override void EndScenario(bool playerWon)
        {
            if (playerWon)
            {
                Debug.Log("Army Battle Scenario: Player Won");
            }
            else
            {
                Debug.Log("Army Battle Scenario: Player Lost");
            }
        }

        public override void StartScenario()
        {
            Debug.Log("Army Battle Scenario: Start");
            ArmyBattleSystem.StartBattleGameplay();
        }

        public override void UpdateScenario(int turnIndex)
        {
            Debug.Log($"Army Battle Scenario: Turn {turnIndex}");
            ArmyBattleSystem.StartNewTurn();
        }
    }
}
