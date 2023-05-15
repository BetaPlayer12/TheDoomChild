namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyCombatHandle
    {
        ArmyAttack attackInfo { get; }
        void AddExtraAttack(int extraAttackCount);
        void SkipTurn();
        void Reset();
    }
}