namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyBattleScenarioTracker
    {
        ArmyBattleTrackerType type { get; }

        void UpdateValue();
    }
}
