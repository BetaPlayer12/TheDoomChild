using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using DChild.Menu;
using Holysoft.Event;
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
        ItemData requiredItem = m_weaponUpgradeData[0].info.weaponUpgradeRequirement[0].item;
        int requiredItemAmount = m_weaponUpgradeData[0].info.weaponUpgradeRequirement[0].amount;
        if (playerInventory.GetCurrentAmount(requiredItem) >= requiredItemAmount)
        {
            return m_weaponUpgradeData[0].info.hasUpgradeRequirements = true;
        }
        else
        {
            return m_weaponUpgradeData[0].info.hasUpgradeRequirements = false;
        }

        //bro check what if null
    }

    public void ExecuteUpgrade(PlayerWeapon playerWeapon, PlayerInventory playerInventory)
    {
        Damage additionalDamage = playerWeapon.damage;
        if(m_weaponUpgradeData[0].info.hasUpgradeRequirements)
        {
            Debug.Log("Yay Upgrade");

            additionalDamage.type = playerWeapon.damage.type;
            additionalDamage.value = playerWeapon.damage.value + m_weaponUpgradeData[0].info.attackdamage.damage.value;
            playerInventory.RemoveItem((m_weaponUpgradeData[0].info.weaponUpgradeRequirement[0].item), m_weaponUpgradeData[0].info.weaponUpgradeRequirement[0].amount);
            playerWeapon.SetBaseDamage(additionalDamage);
        }
        else
        {
            Debug.Log("No upgrade for u");
        }
    }

    public void LoadUpgrade(PlayerWeapon playerWeapon)
    {

    }
}
