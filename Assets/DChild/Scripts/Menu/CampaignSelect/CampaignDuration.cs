using DChild.Serialization;

namespace DChild.Menu.Campaign
{
    public class CampaignDuration : CampaignInfoLabel
    {
        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            var duration = eventArgs.duration;
            m_target.text = $"{duration.hours.ToString("00")}:{duration.minutes.ToString("00")}:{duration.seconds.ToString("00")}";
        }
    }
}