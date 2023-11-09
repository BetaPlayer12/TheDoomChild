using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Menu;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeHandle : MonoBehaviour
{
    [SerializeField]
    private WeaponUpgradeData[] m_weaponUpgradeData;
    [SerializeField]
    private ConfirmationRequestHandle m_confirmationRequest;
    [SerializeField]
    private PlayerInventory m_playerInventory;
    [SerializeField]
    private PlayerWeapon m_playerWeapon;
    [SerializeField]
    private IShardCompletionHandle m_completionHandle;
    protected static Player m_player;

    public void RequestUpgrade()
    {
        IsViableForUpgrade(m_playerWeapon, m_playerInventory);
        m_confirmationRequest.Execute(OnUpgradeConfirm);
    }
    private void OnUpgradeConfirm(object sender, EventActionArgs eventArgs)
    {
        ExecuteUpgrade(m_playerWeapon, m_playerInventory);
    }

    public bool IsViableForUpgrade(PlayerWeapon playerWeapon, PlayerInventory playerInventory)
    {
        WeaponLevel nextWeaponLevel = playerWeapon.GetWeaponLevel()+1;
        bool hasRequirements = false;
        
        for(int i = 0; i < m_weaponUpgradeData[(int)nextWeaponLevel].info.weaponUpgradeRequirement.Length; i++)
        {
            ItemData currentRequiredItem = m_weaponUpgradeData[(int)nextWeaponLevel].info.weaponUpgradeRequirement[i].item;
            int currentRequiredItemAmount = m_weaponUpgradeData[(int)nextWeaponLevel].info.weaponUpgradeRequirement[i].amount;

            if (playerInventory.GetCurrentAmount(currentRequiredItem) >= currentRequiredItemAmount)
            {
                hasRequirements = true;
            }
            else
            {
                hasRequirements = false;
                break;
            }
        }

        m_weaponUpgradeData[(int)nextWeaponLevel].info.hasUpgradeRequirements = hasRequirements;
        Debug.Log("Player has all requirements for next level " + m_weaponUpgradeData[(int)nextWeaponLevel].info.hasUpgradeRequirements);
        return hasRequirements;
    }

    //Effectively this is where we uh when ang item ang mga items needed turns into the actual upgrade
    public void ExecuteUpgrade(PlayerWeapon playerWeapon, PlayerInventory playerInventory)
    {
        Damage additionalDamage = playerWeapon.damage;
        WeaponLevel nextWeaponLevel = playerWeapon.GetWeaponLevel() + 1;
        if (m_weaponUpgradeData[(int)nextWeaponLevel].info.hasUpgradeRequirements)
        {
            Debug.Log("Yay Upgrade");

            additionalDamage.type = playerWeapon.damage.type;
            additionalDamage.value = playerWeapon.damage.value + m_weaponUpgradeData[(int)nextWeaponLevel].info.attackdamage.damage.value;

            for(int i = 0; i < m_weaponUpgradeData[(int)nextWeaponLevel].info.weaponUpgradeRequirement.Length; i++)
            {
                playerInventory.RemoveItem(m_weaponUpgradeData[(int)nextWeaponLevel].info.weaponUpgradeRequirement[i].item, m_weaponUpgradeData[(int)nextWeaponLevel].info.weaponUpgradeRequirement[i].amount);
            }        
            
            playerWeapon.SetBaseDamage(additionalDamage);
            playerWeapon.SetWeaponLevel(playerWeapon.currentWeaponLevel + 1);
            FindObjectOfType<Blacksmith>().UpgradeFinished();
        }
        else
        {
            Debug.Log("No upgrade for u");
            FindObjectOfType<Blacksmith>().UpgradeFailed();
        }
    }

    //reapply stat change based on the wweapons upgrade level
    public void LoadUpgrade(PlayerWeapon playerWeapon)
    {
        Damage additionalDamage = playerWeapon.damage;
        WeaponLevel currentWeaponLevel = playerWeapon.GetWeaponLevel();

        additionalDamage.type = playerWeapon.damage.type;
        additionalDamage.value = playerWeapon.damage.value + m_weaponUpgradeData[(int)currentWeaponLevel].info.attackdamage.damage.value;
        playerWeapon.SetBaseDamage(additionalDamage);
    }

    [Button]
    private void ShowWeaponLevel(PlayerWeapon playerWeapon)
    {
        Debug.Log("Current level " + playerWeapon.GetWeaponLevel());
    }

    private void Awake()
    {
        GameplaySystem.campaignSerializer.PostDeserialization += OnGameplayLoad;
    }

    private void OnGameplayLoad(object sender, CampaignSlotUpdateEventArgs eventArgs)
    {
        if (eventArgs.IsPartOfTheUpdate(SerializationScope.Player))
        {
            LoadUpgrade(m_playerWeapon);
        }
    }
}
