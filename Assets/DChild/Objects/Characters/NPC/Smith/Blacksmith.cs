using DChild.Gameplay;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Characters
{
    public class Blacksmith : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private DialogueDatabase m_UpgradeWeaponDatabase;
        [SerializeField, TabGroup("Main", "Cost")]
        private float m_InitialSilverCoinCost, m_SilverCoinAmountIncrease, m_InitialAttackShardCost, m_AttackShardAmountIncrease;
        private int m_currentSilverCoinCosst,m_currentAttackShardCosst;
        [SerializeField, TabGroup("Main", "Cost")]
        private ItemData m_SilverCoinData, m_AttackShardData;
        [SerializeField, TabGroup("Main", "Upgrade")]
        private float m_InitalAttackUp, m_UpgradeValueIncrease;
        [SerializeField]
        private ItemData m_BrokenSwordData;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private bool m_hasDialogue;
        [SerializeField, TabGroup("Main/Reference", "Actions")]
        private UnityEvent m_Upgradefinished, m_UpdgradeNotPossible,m_MaxUpgrade,m_MaxUpgradeAfterUpgrade,m_Refuse;
        /*
        [SerializeField]
        private DialogueSystemTrigger m_upgradeFinishedDialogueTrigger;
        [SerializeField]
        private DialogueSystemTrigger m_upgradeNotPossibleTrigger;
        [SerializeField]
        private DialogueSystemTrigger m_maxUpgradeTrigger;
        [SerializeField]
        private DialogueSystemTrigger m_maxUpgradeNotificationTrigger;
        */
        private bool m_playerMaxUpgradeAchieved = false;
        [SerializeField]
        private WeaponLevel m_maxWeaponLevel;

        private PlayerStats m_playerstats;
        private int m_playerAttackStat;
        private int m_currentWeaponLevel;
        public bool showPrompt => true;

        public string promptMessage => "Upgrade Weapon";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        private void MaxWeaponLevelReachedCheck()
        {
            if (GameplaySystem.playerManager.player.weapon?.GetWeaponLevel() == m_maxWeaponLevel)
            {
                m_playerMaxUpgradeAchieved = true;
                //m_maxUpgradeNotificationTrigger.OnUse();
                m_MaxUpgradeAfterUpgrade?.Invoke();
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
            m_playerstats = (PlayerStats)GameplaySystem.playerManager.player.stats;
            m_playerAttackStat = m_playerstats.GetBaseStat(PlayerStat.Attack);
            if (DialogueLua.GetVariable("hasBrokenSword").AsBool)
            {
                FreeUpgrade();
                DialogueLua.SetVariable("hasBrokenSword", false); 
                GameplaySystem.playerManager.player.inventory.RemoveItem(m_BrokenSwordData, 1);
                //m_upgradeFinishedDialogueTrigger.OnUse();
                return;
            }
            GameplaySystem.gamplayUIHandle.OpenWeaponUpgradeConfirmationWindow();
            
            ////DEMO/TESTING/NEED ITS OWN UI
            //CharacterRecruitmentUI ui = GameplaySystem.gamplayUIHandle.ConfirmationRequest();
            //m_currentWeaponLevel = ((int)GameplaySystem.playerManager.player.weapon.GetWeaponLevel());
            
            
            //ui.AddAdditionalText(m_playerAttackStat+"->"+ (m_playerAttackStat + CalculateDamageIncrease(m_currentWeaponLevel)));

            //m_currentSilverCoinCosst = (int)(m_InitialSilverCoinCost + (m_InitialSilverCoinCost * m_currentWeaponLevel));
            //ui.AddAdditionalText("\nCosts:"+ m_currentSilverCoinCosst + " Silver coins");

            //m_currentAttackShardCosst = (int)(m_InitialAttackShardCost + (m_AttackShardAmountIncrease * m_currentWeaponLevel));
            //ui.AddAdditionalText("\nCosts:" + m_currentAttackShardCosst + " Attack Shards");


            //ui.SetAcceptOffer(AcceptOffer);
            //ui.SetDeclineOffer(DeclineOffer);
            //ui.SetupUI("Upgrade Weapon to level:" + (m_currentWeaponLevel + 1));
            ////CHANGE THIS ^^^^^^^^^^^^ 

            //BaseGameplaySystem.gamplayUIHandle.SendconfirmationSignal();

        }
        public void FreeUpgrade()
        {
            m_playerstats.SetBaseStat(PlayerStat.Attack, m_playerAttackStat + 15);
            Refuse();
        }

        [Button]
        public void GiveTheDamnedBrokenSword()
        {
            DialogueLua.SetVariable("hasBrokenSword", true);
        }
        private void AcceptOffer(object sender, EventActionArgs eventActionArgs)
        {
            
            if (GameplaySystem.playerManager.player.inventory.GetCurrentAmount(m_AttackShardData) < m_currentAttackShardCosst || GameplaySystem.playerManager.player.inventory.GetCurrentAmount(m_SilverCoinData) < m_currentSilverCoinCosst)
            {
                UpgradeFailed();
                return;
            }

            DialogueLua.SetVariable("PlayerWeaponLevel", m_currentWeaponLevel);

            GameplaySystem.playerManager.player.inventory.RemoveItem(m_AttackShardData, m_currentAttackShardCosst);
            GameplaySystem.playerManager.player.inventory.RemoveItem(m_SilverCoinData, m_currentSilverCoinCosst);


            m_playerstats.SetBaseStat(PlayerStat.Attack, m_playerAttackStat+CalculateDamageIncrease(m_currentWeaponLevel));

            GameplaySystem.playerManager.player.weapon.SetWeaponLevel((WeaponLevel)(m_currentWeaponLevel + 1));
            UpgradeFinished();

        }

        private void DeclineOffer(object sender, EventActionArgs eventActionArgs)
        {
            Refuse();
        }

        private int CalculateDamageIncrease(int weaponlevel)
        {
            return (int)(m_InitalAttackUp + (m_UpgradeValueIncrease * weaponlevel));
        }

        public void UpgradeFinished()
        {
            MaxWeaponLevelReachedCheck();
            //m_upgradeFinishedDialogueTrigger.OnUse();
            m_Upgradefinished?.Invoke();
        }

        public void UpgradeFailed()
        {
            //m_upgradeNotPossibleTrigger.OnUse();
            m_UpdgradeNotPossible?.Invoke();
        }

        public void MaxUpgrade()
        {
            //m_maxUpgradeTrigger.OnUse();
            m_MaxUpgrade?.Invoke();
        }

        public void Refuse()
        {
            m_Refuse?.Invoke();
        }

    }

}
