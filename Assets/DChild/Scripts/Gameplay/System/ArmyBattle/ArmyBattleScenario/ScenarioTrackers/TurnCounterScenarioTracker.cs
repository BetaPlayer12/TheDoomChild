using DChild.Gameplay.ArmyBattle;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.ScenarioTrackers
{
    [System.Serializable]
    public struct TurnCounterScenarioTracker : IArmyBattleScenarioTracker
    {
        [ShowInInspector]
        public ArmyBattleTrackerType type => ArmyBattleTrackerType.TurnCounter;

        public void UpdateValue()
        {
            DialogueLua.SetVariable("AB_TurnCounter", ArmyBattleSystem.GetCurrentTurnNumber());
        }
    }
}
