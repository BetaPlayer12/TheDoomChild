using DChild.Serialization;
using Doozy.Runtime.UIManager.Containers;
using Holysoft.UI;
using UnityEngine;

namespace DChild.Menu.Campaign
{
    public class CampaignSlotOptions : CampaignSelectSubElement
    {
        [SerializeField]
        private UIContainer m_newGame;
        [SerializeField]
        private UIContainer m_loadGame;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            if (eventArgs.isNewGame)
            {
                m_newGame.Show();
                m_loadGame.Hide();
            }
            else
            {
                m_newGame.Hide();
                m_loadGame.Show();
            }
        }
    }

}