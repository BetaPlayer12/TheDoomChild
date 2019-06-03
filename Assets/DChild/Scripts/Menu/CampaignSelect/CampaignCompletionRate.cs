using DChild.Serialization;

namespace DChild.Menu.Campaign
{
    public class CampaignCompletionRate : CampaignInfoLabel
    {
        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            m_target.text = eventArgs.completion.ToString("00") + "%";
        }
    }
}