using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Menu;
using DChild.UI;
using Doozy.Runtime.UIManager.Containers;
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
    //[SerializeField]
    //private ConfirmationRequestHandle m_confirmationRequest;

    [SerializeField] private BlacksmithUI m_ui;

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

        //Hack Solution for COnfirmation
        //m_confirmationRequest.ShowView();
        //m_confirmationRequest.Execute(OnUpgradeConfirm);
    }
    private void OnUpgradeConfirm(object sender, EventActionArgs eventArgs)
    {
        ExecuteUpgrade(m_playerWeapon, m_playerInventory);
        //m_confirmationRequest.HideView();
    }

    public bool IsViableForUpgrade(PlayerWeapon playerWeapon, PlayerInventory playerInventory)
    {
        WeaponLevel nextLevel = playerWeapon.GetWeaponLevel() + 1;
        int levelIndex = (int)nextLevel;

        if (levelIndex < 0 || levelIndex >= m_weaponUpgradeData.Length)
        {
            Debug.LogWarning($"No upgrade data exists for {nextLevel}.");
            return false;
        }

        WeaponUpgradeInfo upgradeInfo =
            m_weaponUpgradeData[levelIndex].info;

        WeaponUpgradeRequirement[] requirements =
            upgradeInfo.weaponUpgradeRequirement ?? Array.Empty<WeaponUpgradeRequirement>();

        List<BlacksmithRequirementUI> uiRows =
            m_ui.requirementsUI;

        m_ui.SetSubHeaderLabel(nextLevel);

        bool hasAllRequirements = true;
        bool canDisplayAllRequirements =
            requirements.Length <= uiRows.Count;
        int visibleRequirementCount =
            Mathf.Min(requirements.Length, uiRows.Count);

        for (int i = 0; i < requirements.Length; i++)
        {
            WeaponUpgradeRequirement requirement = requirements[i];

            if (requirement?.item == null)
            {
                Debug.LogError(
                    $"Requirement {i} for {nextLevel} has no item assigned.");
                hasAllRequirements = false;

                if (i < visibleRequirementCount)
                {
                    uiRows[i].gameObject.SetActive(false);
                }

                continue;
            }

            int inventoryQuantity =
                playerInventory.GetCurrentAmount(requirement.item);

            hasAllRequirements &=
                inventoryQuantity >= requirement.amount;

            if (i < visibleRequirementCount)
            {
                BlacksmithRequirementUI row = uiRows[i];

                row.gameObject.SetActive(true);
                row.SetDynamicVisuals(
                    requirement.item,
                    inventoryQuantity,
                    requirement.amount);
            }
        }

        // Hide leftover UI rows from the previous upgrade level.
        for (int i = visibleRequirementCount; i < uiRows.Count; i++)
        {
            uiRows[i].gameObject.SetActive(false);
        }

        if (!canDisplayAllRequirements)
        {
            Debug.LogError(
                $"{nextLevel} has {requirements.Length} requirements, " +
                $"but the UI only has {uiRows.Count} rows.");
        }

        bool isViable =
            hasAllRequirements && canDisplayAllRequirements;

        upgradeInfo.hasUpgradeRequirements = isViable;
        return isViable;
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

            for (int i = 0; i < m_weaponUpgradeData[(int)nextWeaponLevel].info.weaponUpgradeRequirement.Length; i++)
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

        m_ui.OnUpgradeConfirmed -= OnUpgradeConfirm;
        m_ui.OnUpgradeConfirmed += OnUpgradeConfirm;
    }

    private void OnGameplayLoad(object sender, CampaignSlotUpdateEventArgs eventArgs)
    {
        if (eventArgs.IsPartOfTheUpdate(SerializationScope.Player))
        {
            LoadUpgrade(m_playerWeapon);
        }
    }

    private void OnDestroy()
    {
        GameplaySystem.campaignSerializer.PostDeserialization -= OnGameplayLoad;
    }
}
