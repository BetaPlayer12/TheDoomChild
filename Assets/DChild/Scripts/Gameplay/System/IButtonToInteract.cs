using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public interface IButtonToInteract
    {
        bool showPrompt { get; }
        string promptMessage { get; }
        Vector3 promptPosition { get; }
        Transform transform { get; }
        void Interact(Character character);

        event EventAction<EventActionArgs> InteractionOptionChange;
    }
}