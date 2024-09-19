using DChild.Gameplay.ArmyBattle;

public class RandomizedArmyAttack : IArmyAIAction
{
    public bool isRandomizedAction => true;

    ArmyGroupTemplateData IArmyAIAction.GetAction() => null;


}
