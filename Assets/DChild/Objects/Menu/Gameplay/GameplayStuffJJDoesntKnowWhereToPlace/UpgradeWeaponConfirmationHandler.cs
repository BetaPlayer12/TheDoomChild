using DChild.Menu;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWeaponConfirmationHandler : MonoBehaviour
{
    [SerializeField]
    private ConfirmationRequestHandle m_confirmationRequest;
    [SerializeField]
    private WeaponShardConversionHandler m_weaponShardConversion;

    public void RequestUpgrade()
    {
        m_confirmationRequest.Execute(OnUpgradeConfirm);
    }
    private void OnUpgradeConfirm(object sender, EventActionArgs eventArgs)
    {
        Debug.Log("Upgrade Done");
        m_weaponShardConversion.CommenceUpgrade();
    }

}
