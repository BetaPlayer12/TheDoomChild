using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Campaign
{
    public class CampaignLocationImage : CampaignSelectSubElement
    {
        [SerializeField]
        private Image m_targetGraphic;
        [SerializeField]
        private LocationImageList m_list;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            m_targetGraphic.sprite = m_list.GetImageOf(eventArgs.location);
            m_targetGraphic.color = m_targetGraphic.sprite == null ? Color.black : Color.white;
        }
    }
}