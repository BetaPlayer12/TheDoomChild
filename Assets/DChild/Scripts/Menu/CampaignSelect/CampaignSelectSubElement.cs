using DChild.Serialization;
using UnityEngine;

namespace DChild.Menu.Campaign
{
    public abstract class CampaignSelectSubElement : MonoBehaviour
    {
        private ICampaignSelect m_campaignSelect;

        protected abstract void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs);

        protected virtual void Awake()
        {
            m_campaignSelect = GetComponentInParent<ICampaignSelect>();
        }

        //protected virtual void OnEnable()
        //{
        //    m_campaignSelect.CampaignSelected += OnCampaignSelected;
        //}

        //protected virtual void OnDisable()
        //{
        //    m_campaignSelect.CampaignSelected += OnCampaignSelected;
        //}
    }
}