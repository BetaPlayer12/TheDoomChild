using Holysoft.Menu;
using Holysoft.UI;
using Spine;
using UnityEngine;

namespace DChild.Menu.MainMenu
{
    public class SplashScreen : UIStylishCanvas
    {
        [SerializeField]
        private SplashScreenAnimation m_animation;
        [SerializeField]
        private SplashScreenLabelAnimation m_labelAnimation;
        [SerializeField]
        private WindowTransistion m_transistion;

        public override void Show()
        {
            base.Show();
            m_animation.Show();
            m_animation.Initialize();
            m_labelAnimation.Stop();
            enabled = true;
        }

        public override void Hide()
        {
            base.Hide();
            m_animation.Hide();
            enabled = false;
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_animation.startAnimation)
            {
                m_transistion.StartTransistion();
            }
        }
        private void Start()
        {
            //m_animation.zSkeleton.state.Complete += OnComplete;
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                m_animation.TransistionAnimation();
                m_labelAnimation.Play();
                enabled = false;
            }
        }
    }
}