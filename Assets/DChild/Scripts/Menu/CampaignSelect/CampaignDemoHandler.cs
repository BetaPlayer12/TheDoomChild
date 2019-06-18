using DChild.Serialization;
using UnityEngine;

namespace DChild.Menu.Campaign
{
    public class CampaignDemoHandler : CampaignSelectSubElement
    {
        [System.Serializable]
        public class CanvasSelection
        {
            [SerializeField]
            private Canvas m_demo;

            [SerializeField]
            private Canvas m_nonDemo;

            public void ChangeTo(bool demo)
            {
                m_demo.enabled = demo;
                m_nonDemo.enabled = !demo;
            }
        }

        [SerializeField]
        private CanvasSelection m_locationLabel;
        [SerializeField]
        private CanvasSelection m_options;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            var isDemoGame = eventArgs.isDemoGame;
            m_locationLabel.ChangeTo(eventArgs.isDemoGame);
            m_options.ChangeTo(eventArgs.isDemoGame);
        }
    }
}