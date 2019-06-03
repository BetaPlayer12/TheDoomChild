using Holysoft.UI;
using UnityEngine;

namespace DChild.Menu
{
    public class CreditsScreen : UICanvas
    {
        [SerializeField]
        private CreditReel m_reel;

        public override void Hide()
        {
            base.Hide();
            m_reel.Stop();
        }

        public override void Show()
        {
            base.Show();
            m_reel.Play();
        }
    }
}