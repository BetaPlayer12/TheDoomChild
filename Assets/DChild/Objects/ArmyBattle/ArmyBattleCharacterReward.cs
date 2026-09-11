using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Trade;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static DChild.Gameplay.UnlockableEvent;

namespace DChild.Gameplay.ArmyBattle
{
    public enum RequirementType
    {
        SoulEssence,
        Item,
        CombatArt,
        PrimarySkill,
        SpecificRecruit,
        ArmySize,
        Other
    }

    public class ArmyBattleCharacterReward : MonoBehaviour
    {
       
        //[TabGroup("Main","Reference")]
        //[SerializeField, TabGroup("Main/Reference", "General References")]
        //private SpriteRenderer m_Graphics;
        //[SerializeField, TabGroup("Main/Reference", "General References")]
        //private Vector3 m_promptOffset;
        [SerializeField, TabGroup("Main/Reference", "General References")]
        private CharacterGiver m_CharacterGiver;


        [SerializeField, TabGroup("Main/Reference", "Actions")]
        private UnityEvent m_GiveReward,m_RequirementFailed;
        [SerializeField, TabGroup("Main/Reference","Character Reward")]
        private List<ArmyCharacterData> m_CharacterReward;


        [SerializeField, TabGroup("Main","Requirements")]
        private bool m_isFree;
        [SerializeField, TabGroup("Main", "Requirements"),HideIf("m_isFree")]
        private RequirementType m_Requirement;
        [SerializeField, TabGroup("Main", "Requirements"), Tooltip("Takes the item from the player's invintory if possible"), HideIf("m_isFree")]
        private bool m_TakeRequiredItemFromInvintory;
        [SerializeField, TabGroup("Main", "Requirements"), HideIf("m_isFree"),ShowIf("m_Requirement",RequirementType.SoulEssence)]
            private int m_requiredSoulEssence;

        [SerializeField, TabGroup("Main","Requirements"), HideIf("m_isFree"), ShowIf("m_Requirement", RequirementType.Item)]
            private ItemData m_hasItem;
        [SerializeField, TabGroup("Main", "Requirements"), HideIf("m_isFree"), ShowIf("m_Requirement", RequirementType.Item), Min(1)]
            private int m_ItemAmount = 1;

        [SerializeField, TabGroup("Main", "Requirements"), HideIf("m_isFree"), ShowIf("m_Requirement", RequirementType.CombatArt)]
            private CombatArt m_CombatArt;

        [SerializeField, TabGroup("Main", "Requirements"), HideIf("m_isFree"), ShowIf("m_Requirement", RequirementType.PrimarySkill)]
            private PrimarySkill m_PrimarySkill;

        [SerializeField, TabGroup("Main", "Requirements"), HideIf("m_isFree"), ShowIf("m_Requirement", RequirementType.SpecificRecruit)]
            private ArmyCharacterData[] armyCharacterData;

        [SerializeField, TabGroup("Main", "Requirements"), HideIf("m_isFree"), ShowIf("m_Requirement", RequirementType.ArmySize)]
            private int neededNPCsRecruited;

        private bool m_OtherConditions;

        private bool m_RequirementAchieved;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public void SendRecruitmentConfirmationUI()
        {
            CharacterRecruitmentUI ui = GameplaySystem.gamplayUIHandle.ConfirmationRequest();
            ui.SetAcceptOffer(AcceptOffer);
            ui.SetDeclineOffer(DeclineOffer);
            ui.SetupUI(m_CharacterReward[0].name);
            BaseGameplaySystem.gamplayUIHandle.SendconfirmationSignal();
        }

        public void RequirementMet(bool isAchieved)
        {
            m_RequirementAchieved = isAchieved;
        }

        public void HasOtherConditions(bool x)
        {
            m_OtherConditions = x;
        }

        public void SetupConfirmationUI()
        {
            CharacterRecruitmentUI ui = GameplaySystem.gamplayUIHandle.ConfirmationRequest();
            if (!m_isFree)
            {
                /*
                if(m_requiresSoulEssence)
                {
                    ui.AddSoulessenceReq(m_requiredSoulEssence);
                }
                if(m_requiresItem)
                {
                    ui.AddItemReq(m_hasItem,m_ItemAmount);
                }
                if(m_requiresPrimarySkill)
                {
                    ui.AddPrimarySkillReq(m_PrimarySkill);
                }
                if(m_requiresCombatArt)
                {
                    ui.AddCombatArtReq(m_CombatArt);
                }
                if(m_requiresSpecificNPC)
                {
                    ui.AddNPCRecruitedReq(armyCharacterData);
                }
                if(m_requiresMinimumNPCsRecruited)
                {
                    ui.AddArmySizeReq(neededNPCsRecruited);
                }*/
                switch(m_Requirement)
                {
                    case RequirementType.SoulEssence:
                        ui.AddSoulessenceReq(m_requiredSoulEssence);
                        break;

                    case RequirementType.Item:
                        ui.AddItemReq(m_hasItem, m_ItemAmount);
                        break;

                    case RequirementType.PrimarySkill:
                        ui.AddPrimarySkillReq(m_PrimarySkill);
                        break;

                    case RequirementType.CombatArt:
                        ui.AddCombatArtReq(m_CombatArt);
                        break;

                    case RequirementType.SpecificRecruit:
                        if (armyCharacterData.Count() >= 1)
                        {
                            foreach (ArmyCharacterData army in armyCharacterData)
                            {
                                ui.AddNPCRecruitedReq(army);
                            }
                        }
                        else
                        {
                            ui.AddNPCRecruitedReq(armyCharacterData[0]);
                        }
                        //ui.AddNPCRecruitedReq(armyCharacterData);
                        break;

                    case RequirementType.ArmySize:
                        ui.AddArmySizeReq(neededNPCsRecruited);
                        break;
                }
                
                if(m_OtherConditions)
                {
                    //reserverd for special cases
                }
            }
        }

        [Button]
        public void RecruitmentConfirmation()
        {
            SetupConfirmationUI();
            SendRecruitmentConfirmationUI();
            GameSystem.SetCursorVisibility(true);
        }
        private void DeclineOffer(object sender, EventActionArgs eventActionArgs)
        {
            GameSystem.SetCursorVisibility(false);
        }

        private void AcceptOffer(object sender, EventActionArgs eventActionArgs)
        {
            GameSystem.SetCursorVisibility(false);

            if (!m_isFree)
            {
                if(EvaluateRequirements() == false)
                {
                    RequirementFailed();
                    return; //return guard here to prevent rewarding character
                }
                else //if player meets requirements, check if you need to remove items from him
                {
                    if (m_TakeRequiredItemFromInvintory)
                    {
                        switch (m_Requirement)
                        {
                            case RequirementType.SoulEssence:
                                GameplaySystem.playerManager.player.inventory.AddSoulEssence(-m_requiredSoulEssence);
                                break;
                            case RequirementType.Item:
                                var quickItem = GameplaySystem.playerManager.player.inventory.quickItemInventory;
                                if(quickItem != null)
                                {
                                    if(quickItem.GetItem(m_hasItem) != null)
                                    {
                                        if (quickItem.GetItem(m_hasItem).count >= m_ItemAmount)
                                        {
                                            quickItem.RemoveItem(m_hasItem, m_ItemAmount);
                                        }
                                    }
                                  
                                }
                                GameplaySystem.playerManager.player.inventory.RemoveItem(m_hasItem, m_ItemAmount);
                                break;
                        }
                    }
                }
            }

            GiveRewardNow();
        }

        private bool EvaluateRequirements()
        {
            //Check other conditions first  because they are unique and should have more priority
            if (m_OtherConditions)
            {
                if (!m_RequirementAchieved)
                {
                    return false;
                }
            }

            //if there are no other special conditions, check standard conditions
            switch (m_Requirement)
            {
                case RequirementType.SoulEssence:
                    if (GameplaySystem.playerManager.player.inventory.GetCurrencyAmount(CurrencyType.SoulEssence) < m_requiredSoulEssence && m_requiredSoulEssence != 0)
                        return false;
                    break;

                case RequirementType.Item:
                    int x = GameplaySystem.playerManager.player.inventory.GetCurrentAmount(m_hasItem);
                    var quickItemCheck = GameplaySystem.playerManager.player.inventory.quickItemInventory;
                    if (x == 0 || x < m_ItemAmount)
                    {
                        var quickItemInventory = quickItemCheck.GetItem(m_hasItem)?.count ?? 0;
                          if(quickItemInventory == 0 || quickItemInventory < m_ItemAmount)
                        {
                            return false;
                        }
                                         
                    }
                        
                    break;

                case RequirementType.CombatArt:
                    if (!GameplaySystem.playerManager.player.combatArts.IsAbilityActivated(m_CombatArt))
                        return false;
                    break;

                case RequirementType.PrimarySkill:
                    if (!GameplaySystem.playerManager.player.skills.IsSkillUnlocked(m_PrimarySkill))
                        return false;
                    break;

                case RequirementType.SpecificRecruit:
                    if (armyCharacterData.Count() >= 1)
                    { 
                        foreach(ArmyCharacterData army in armyCharacterData)
                        {
                            if (!GameplaySystem.playerManager.armyBattleCharacterRecruiter.HasRecruitedCharacter(army))
                                return false;
                        }
                    }else
                    {
                        if (!GameplaySystem.playerManager.armyBattleCharacterRecruiter.HasRecruitedCharacter(armyCharacterData[0]))
                            return false;
                    }
                    
                    break;

                case RequirementType.ArmySize:
                    if (GameplaySystem.playerManager.armyBattleCharacterRecruiter.ArmySize() < neededNPCsRecruited)
                        return false;
                    break;
                default:
                    break;
            }

            //if none of the cases above return false, assume that requirements are mets return true
            return true;
        }

        private void RequirementFailed()
        {
            m_RequirementFailed?.Invoke();
        }

        /*
        private string ConstructRequirement()
        {
            string m_RequirementsText = m_CharacterReward[0].name;
            if (!m_isFree&&m_TakeRequiredItemFromInvintory)
            {
                m_RequirementsText += "\nRequirements:";
                if(m_requiresSoulEssence)
                {
                    m_RequirementsText += "\n•<color=yellow>" + m_requiredSoulEssence + "</color> Soul Essence";
                }
                if(m_requiresItem)
                {
                    m_RequirementsText += "\n•" + m_ItemAmount.ToString()+ " <color=yellow>" + m_hasItem.itemName+ "</color>";
                }
                if(m_requiresPrimarySkill)
                {
                    m_RequirementsText += "\n•Aquired:"+ m_PrimarySkill.ToString();
                }
                if(m_requiresCombatArt)
                {
                    m_RequirementsText += "\n•Learned:" + m_CombatArt.ToString();
                }
                if(m_requiresSpecificNPC)
                {
                    m_RequirementsText += "\n•Recruited:" + armyCharacterData.name;
                }
                if(m_requiresMinimumNPCsRecruited)
                {
                    m_RequirementsText += "\n•Has " + neededNPCsRecruited + " recruited";
                }
            }
            m_RequirementsText += "\n Accept?";
            return m_RequirementsText;
        }*/
        [Button]
        public void GiveRewardNow()
        {
            m_GiveReward?.Invoke();
            m_CharacterGiver?.RecruitCharacter(m_CharacterReward);
            //Because Characters are usually recieved at isolated maps where save points do not exists
            //and Underworld data is lost upon exiting due to changing into Overworld Data 
            GameplaySystem.campaignSerializer.UpdateData(SerializationScope.Quest);
        }
        private void OnDecline(object sender, EventActionArgs eventActionArgs)
        {

        }

    }
}

