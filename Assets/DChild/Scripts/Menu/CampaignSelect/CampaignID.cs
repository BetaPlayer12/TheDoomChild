using DChild.Serialization;

namespace DChild.Menu.Campaign
{
    public class CampaignID : CampaignInfoLabel
    {
        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            m_target.text = eventArgs.ID.ToString("00");
        }
    }
}