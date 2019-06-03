using Holysoft.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Menu.Campaign
{
    public class CampaignSelectIntroAnimation : UIElement3D
    {
        [SerializeField]
        private PlayableDirector m_intro;

        public override void Show()
        {
            m_intro.Play();
        }

        public override void Hide()
        {
        }
    }
}