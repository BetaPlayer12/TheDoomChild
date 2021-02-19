using DChild.Gameplay.Environment.Interractables;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DChild.Gameplay.Characters.NPC
{
    public class NPCDialogue : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private bool m_hasDialogue = true;
        [SerializeField]
        private Transform m_promptPosition;

        private DialogueSystemTrigger m_trigger;

        public bool hasDialogue
        {
            set => m_hasDialogue = value;
        }

        public bool showPrompt => m_hasDialogue;

        public string promptMessage => "True";

        public Vector3 promptPosition => m_promptPosition.position;

        public void Interact(Character character)
        {
            m_trigger.OnUse(character.transform);
        }

        private void Awake()
        {
            m_trigger = GetComponent<DialogueSystemTrigger>();
        }
    }
}