using DChild.Gameplay.ArmyBattle.ScenarioTrackers;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public static class ArmyBattleScenarioTrackerCreator
    {
        public static IArmyBattleScenarioTracker CreateTracker(ArmyBattleTrackerType trackerType)
        {
            switch (trackerType)
            {
                case ArmyBattleTrackerType.TurnCounter:
                    return new TurnCounterScenarioTracker();
                case ArmyBattleTrackerType.TroopCount:
                    return new TroopCountScenarioTracker();
                default:
                    Debug.LogError($"{trackerType} does not have appropriate Tracker Instance");
                    return null;
            }
        }
    }
}
