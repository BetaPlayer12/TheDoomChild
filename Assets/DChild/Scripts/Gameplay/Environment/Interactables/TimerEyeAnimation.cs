using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{

    public class TimerEyeAnimation : MonoBehaviour
    {
        [SerializeField]
        private DelayedEventHandle m_handle;
        [SerializeField]
        private SpineAnimation m_spineAnimation;
        [SerializeField]
        private TimerEyeAnimationData m_animationData;

        private int m_closingIntervals;
        private int m_currentClosingInterval;
        private float m_closingIntervalTime;
        private float m_nextClosingTime;


        public void UseOpenAnimation()
        {
            m_spineAnimation.SetAnimation(0, m_animationData.openAnimation, false);
            enabled = false;
        }

        public void StartClosingAnimation()
        {
            enabled = true;
            m_currentClosingInterval = 1;
            m_nextClosingTime = m_handle.delayTime - (m_closingIntervalTime * m_currentClosingInterval);
        }

        public void UseCloseAnimation()
        {
            m_spineAnimation.SetAnimation(0, m_animationData.closeAnimation, false);
            enabled = false;
        }

        private void Awake()
        {
            m_closingIntervals = m_animationData.closeIntervalAnimationList.Length;
            m_currentClosingInterval = 0;
            m_closingIntervalTime = m_handle.delayTime / (m_closingIntervals + 1);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (m_handle.currentDelayTimer > 0)
            {
                if (m_handle.currentDelayTimer <= m_nextClosingTime)
                {
                    m_spineAnimation.SetAnimation(0, m_animationData.closeIntervalAnimationList[m_currentClosingInterval - 1], false);
                    m_currentClosingInterval++;
                    m_nextClosingTime = m_handle.delayTime - (m_closingIntervalTime * m_currentClosingInterval);
                }
            }
        }

        private void OnValidate()
        {
            enabled = false;
        }
    }
}
