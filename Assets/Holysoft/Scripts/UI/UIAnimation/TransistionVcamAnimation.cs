using Cinemachine;
using Holysoft.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.UI
{
    public class TransistionVcamAnimation : UIAnimation
    {
        [SerializeField]
        protected Animator m_animation;
        [SerializeField]
        private string m_animationName;

        private float m_timer;
        private float m_animationDuration;

        public override void Pause()
        {
            enabled = false;
        }

        public override void Play()
        {
            enabled = true;
            m_timer = 0;
            m_animation.Play(m_animationName, 0);
            Debug.Log($"{gameObject.name} Called {m_animationName}");
        }

        public override void Stop()
        {
            m_timer = 0;
            enabled = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_animationDuration = 1;
        }

        private void Update()
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_animationDuration)
            {
                CallAnimationEnd();
                enabled = false;
            }
        }
    }
}
