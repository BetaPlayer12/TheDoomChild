using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Inventories;
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
        m_confirmationRequest.Execute(OnUpgradeConfirm);
    }
    private void OnUpgradeConfirm(object sender, EventActionArgs eventArgs)
    {
        ExecuteUpgrade(m_playerWeapon, m_playerInventory);
    }

    public bool IsViableForUpgrade(PlayerWeapon playerWeapon, PlayerInventory playerInventory)
    {
        bool isViable;

        if (m_weaponUpgradeData[0].info.hasUpgradeRequirements)
        {
            return isViable = true;
        }
        else
        {
            return isViable = false;
        }
        
    }

    public void ExecuteUpgrade(PlayerWeapon playerWeapon, PlayerInventory playerInventory)
    {
        Damage additionalDamage = m_playerWeapon.damage;
        if(IsViableForUpgrade(playerWeapon, playerInventory))
        {
            Debug.Log("Yay Upgrade");

            additionalDamage.value = m_playerWeapon.damage.value + m_weaponUpgradeData[0].info.attackdamage.damage.value;
            m_playerInventory.RemoveItem((m_weaponUpgradeData[0].info.weaponUpgradeRequirement[0].item));
            m_playerWeapon.SetBaseDamage(additionalDamage);
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
