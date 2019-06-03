using Holysoft.Collections;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace DChild.Menu.MainMenu
{
    public class SplashScreenAnimation : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_zSkeleton;
        [SerializeField]
        [SpineAnimation]
        private string m_idleLoop;
        [SerializeField]
        [SpineAnimation]
        private string m_startAnimation;


        public SkeletonAnimation zSkeleton => m_zSkeleton;
        public string startAnimation => m_startAnimation;

        public void Initialize()
        {
            gameObject.SetActive(true);
            if (m_zSkeleton.state != null)
            {
                var track = m_zSkeleton.state.SetAnimation(0, m_idleLoop, true);
                track.MixDuration = 0;
            }
        }

        public void TransistionAnimation()
        {
            m_zSkeleton.state.SetAnimation(0, m_startAnimation, false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}