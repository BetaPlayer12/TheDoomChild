using DChild.Gameplay.ArmyBattle;
public interface IArmyAIAction
{
    public ArmyGroupData GetAction();
    bool isRandomizedAction { get; }

}
