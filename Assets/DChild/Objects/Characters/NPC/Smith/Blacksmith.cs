using DChild.Gameplay;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
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
        private DialogueSystemTrigger m_upgradeFinishedDialogueTrigger;
        [SerializeField]
        private DialogueSystemTrigger m_upgradeNotPossibleTrigger;
        [SerializeField]
        private DialogueSystemTrigger m_maxUpgradeTrigger;
        [SerializeField]
        private DialogueSystemTrigger m_maxUpgradeNotificationTrigger;
        private bool m_playerMaxUpgradeAchieved = false;
        [SerializeField]
        private WeaponLevel m_maxWeaponLevel;

        public bool showPrompt => true;

        public string promptMessage => "Upgrade Weapon";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        private void MaxWeaponLevelReachedCheck()
        {
            if (GameplaySystem.playerManager.player.weapon.GetWeaponLevel() == m_maxWeaponLevel)
            {
                m_playerMaxUpgradeAchieved = true;
                m_maxUpgradeNotificationTrigger.OnUse();
            }
        }

        public void Interact(Character character)
        {
            if (m_hasDialogue)
            {
                if (m_playerMaxUpgradeAchieved)
                {
                    MaxUpgrade();
                }
                else
                {
                    GetComponent<NPCDialogue>().Interact(character);
                }
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

        public void UpgradeFinished()
        {
            MaxWeaponLevelReachedCheck();
            m_upgradeFinishedDialogueTrigger.OnUse();
        }

        public void UpgradeFailed()
        {
            m_upgradeNotPossibleTrigger.OnUse();
        }

        public void MaxUpgrade()
        {
            m_maxUpgradeTrigger.OnUse();
        }

    }

}
