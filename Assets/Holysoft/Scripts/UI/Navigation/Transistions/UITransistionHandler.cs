using System;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public class UITransistionHandler : UITransistion
    {
        [SerializeField]
        private UIAnimation[] m_transistionAnimations;
        private int m_unfinishAnimationCount;

        public override void Play()
        {
            for (int i = 0; i < m_transistionAnimations.Length; i++)
            {
                m_transistionAnimations[i].Play();
            }
        }
        public override void Pause()
        {
            for (int i = 0; i < m_transistionAnimations.Length; i++)
            {
                m_transistionAnimations[i].Pause();
            }
        }
        public override void Stop()
        {
            m_unfinishAnimationCount = m_transistionAnimations.Length;
            for (int i = 0; i < m_transistionAnimations.Length; i++)
            {
                m_transistionAnimations[i].Stop();
            }
        }

        private void OnAnimationEnd(object sender, EventActionArgs eventArgs)
        {
            m_unfinishAnimationCount--;
            if (m_unfinishAnimationCount == 0)
            {
                CallTransistionEnd();
                Stop();
            }
        }

        protected override void Awake()
        {
            m_unfinishAnimationCount = m_transistionAnimations.Length;
            for (int i = 0; i < m_transistionAnimations.Length; i++)
            {
                m_transistionAnimations[i].AnimationEnd += OnAnimationEnd;
            }
            base.Awake();
        }


#if UNITY_EDITOR
        [Button("Get Children Animations")]
        private void GetChildrenHAnimation()
        {
            m_transistionAnimations = GetComponentsInChildren<UIAnimation>();
        }
#endif
    }
}