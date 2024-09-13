using DChild.Gameplay.ArmyBattle;

public class RandomizedArmyAttack : IArmyAIAction
{
    public bool isRandomizedAction => true;

    ArmyGroupData IArmyAIAction.GetAction() => null;


}
