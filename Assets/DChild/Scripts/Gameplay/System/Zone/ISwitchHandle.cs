using DChild.Gameplay;

public enum TransitionType
{
    Enter,
    PostEnter,
    Exit
}

public interface ISwitchHandle
{
    void DoSceneTransition(Character character, TransitionType type);
    float transitionDelay { get; }
}
