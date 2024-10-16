using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.ArmyBattle.ScenarioTrackers
{
    [System.Serializable]
    public struct TroopCountScenarioTracker : IArmyBattleScenarioTracker
    {
        [ShowInInspector]
        public ArmyBattleTrackerType type => ArmyBattleTrackerType.TroopCount;

        public void UpdateValue()
        {
            DialogueLua.SetVariable("AB_TroopCountPlayer", ArmyBattleSystem.GetPlayer().controlledArmy.troopCountPercent);
            DialogueLua.SetVariable("AB_TroopCountEnemy", ArmyBattleSystem.GetEnemy().controlledArmy.troopCountPercent);
        }
    }
}
