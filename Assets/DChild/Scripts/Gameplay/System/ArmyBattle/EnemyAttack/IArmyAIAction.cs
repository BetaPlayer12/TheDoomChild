using DChild.Gameplay.ArmyBattle;
public interface IArmyAIAction
{
    public ArmyGroupTemplateData GetAction();
    bool isRandomizedAction { get; }

}
