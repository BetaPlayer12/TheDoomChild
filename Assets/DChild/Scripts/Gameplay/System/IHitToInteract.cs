using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{

    public interface IHitToInteract : IInteractable
    {
        void Interact();
    }
}