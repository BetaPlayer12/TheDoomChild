using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public interface IInteractable
    {
        Vector3 position { get; }
        IInteractable Interact(IInteractingAgent agent);
    }
}