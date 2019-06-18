using DChild.Gameplay.Environment;
using DChild.Serialization;
using Holysoft;
using Refactor.DChild.Menu;
using TMPro;

namespace DChild.Menu.Campaign
{
    public class CampaignLocation : CampaignInfoLabel
    {
        private Location m_currentLocation;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            if (m_currentLocation != eventArgs.location)
            {
                m_currentLocation = eventArgs.location;
                var locationString = m_currentLocation.ToString();
                locationString = locationString.ToUpper();
                locationString = locationString.Replace('_', ' ');
                m_target.text = locationString;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_currentLocation = Location._COUNT;
        }
    }
}