/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Environment.Interractables;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class ShadowGate : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private Location m_fromLocation;
        [SerializeField]
        private Transform m_prompt;

        public bool showPrompt => true;

        public string promptMessage => "Use";

        public Vector3 promptPosition => m_prompt.position;

        public void Interact(Character character)
        {
            GameplaySystem.gamplayUIHandle.OpenWorldMap(m_fromLocation);
        }
    }
}