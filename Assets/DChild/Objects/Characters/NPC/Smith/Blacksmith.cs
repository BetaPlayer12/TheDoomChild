using DChild.Gameplay;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class Blacksmith : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private bool m_hasDialogue;
        [SerializeField]
        private bool m_signalSender;

        public bool showPrompt => true;

        public string promptMessage => "Upgrade Weapon";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public void Interact(Character character)
        {
            if (m_hasDialogue)
            {
                GetComponent<NPCDialogue>().Interact(character);
            }
            else
            {
                CommenceUpgrade();
            }
        }

        public void CommenceUpgrade()
        {
            GameplaySystem.gamplayUIHandle.OpenWeaponUpgradeConfirmationWindow();
        }
    }

}
