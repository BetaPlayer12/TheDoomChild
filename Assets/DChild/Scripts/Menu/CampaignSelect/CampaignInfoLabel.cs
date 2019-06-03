using Holysoft;
using TMPro;
using UnityEngine;

namespace DChild.Menu.Campaign
{
    public abstract class CampaignInfoLabel : CampaignSelectSubElement
    {
        [SerializeField]
        protected TextMeshProUGUI m_target;

        protected void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_target, ComponentUtility.ComponentSearchMethod.Child);
        }
    }
}