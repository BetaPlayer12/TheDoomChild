namespace DChild.Gameplay.Characters.AI
{
    public interface ITerrainPatroller
    {
        bool waitForBehaviourEnd { get; }
        void Turn();
        void Move();
    }
}