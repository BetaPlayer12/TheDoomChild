using DChild.Serialization;
using UnityEngine;

namespace DChild.Menu.Campaign
{
    public class CampaignCompletionRate : CampaignInfoLabel
    {
        [SerializeField]
        private Canvas m_canvas;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            var enableCanvas = !eventArgs.isNewGame;
            m_canvas.enabled = enableCanvas;
            if (enableCanvas)
            {
                m_target.text = eventArgs.completion.ToString("00") + "%";
            }
        }
    }
}