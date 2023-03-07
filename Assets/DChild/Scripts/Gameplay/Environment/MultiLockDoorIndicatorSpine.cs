using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    [RequireComponent(typeof(SkeletonAnimation))]
    public class MultiLockDoorIndicatorSpine : MultiLockDoorIndicator
    {
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_closedAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string m_openedAnimation;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string[] m_otherIndicator;

        private bool m_skeletonInitialized;
        private SpineAnimation m_animation;


        protected override void SetIndication(int currentUnlockCount, bool isOpen)
        {
            if (m_skeletonInitialized == false)
                return;

            if (isOpen)
            {
                m_animation.SetAnimation(0, m_otherIndicator[m_otherIndicator.Length - 1], false);
            }
            else
            {
                if (currentUnlockCount == 0)
                {
                    m_animation.SetAnimation(0, m_closedAnimation, false);
                }
                else
                {
                    m_animation.SetAnimation(0, m_otherIndicator[currentUnlockCount - 1], false);
                }
            }
        }

        private void SetIndicationAs(int currentUnlockCount, bool isOpen)
        {
            if (isOpen)
            {
                m_animation.SetAnimation(0, m_openedAnimation, false);
            }
            else
            {
                if (currentUnlockCount == 0)
                {
                    m_animation.SetAnimation(0, m_closedAnimation, false);
                }
                else
                {
                    m_animation.SetAnimation(0, m_otherIndicator[currentUnlockCount - 1], false);
                }
            }
        }

        private void Start()
        {
            m_skeletonInitialized = true;
            m_animation = GetComponent<SpineAnimation>();
            SetIndicationAs(m_reference.currentUnlockCount, m_reference.isOpen);
        }
    }

}