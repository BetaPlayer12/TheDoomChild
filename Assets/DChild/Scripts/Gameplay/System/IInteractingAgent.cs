using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public interface IInteractingAgent
    {
        Transform transform { get; }
        T GetComponent<T>() where T : Component;
    }
}