namespace DChild.Gameplay.Environment.Interractables
{
    public interface IInteractionRequirement
    {
        string requirementMessage { get; }
        bool CanBeInteracted(Character character);
    }
}