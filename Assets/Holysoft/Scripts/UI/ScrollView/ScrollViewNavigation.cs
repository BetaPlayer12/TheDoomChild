using System;
using Holysoft.Collections;
using Holysoft.Event;
using Holysoft.Menu;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    public class ScrollViewNavigation : AdjacentNavigation
    {
        [SerializeField]
        private Scrollbar m_scrollBar;
        [SerializeField]
        [MinValue(1)]
        private int m_steps;
        [SerializeField]
        private LerpDuration m_lerpDuration;
        private float m_intervalDistance;

        public event EventAction<EventActionArgs> LerpStart;
        public event EventAction<EventActionArgs> LerpEnd;

        private float m_toValue;
        private float m_fromValue;

        protected override int lastNavigationIndex => m_steps;

        public void JumpTo(int index)
        {
            //if (m_currentNavigationIndex != index)
            //{
            //    m_currentNavigationIndex = index;
            //    m_scrollBar.value = m_intervalDistance * m_currentNavigationIndex;
            //}
            //Debug.Log(m_scrollBar.value);
            //if (m_currentNavigationIndex == 0)
            //{
            //    CallFirstItemReached();
            //}
            //else if (m_currentNavigationIndex == lastNavigationIndex)
            //{
            //    CallLastItemReached();
            //}
            //else
            //{
            //    CallNavigatingItem();
            //}
        }

        private void Lerp(float from, float to)
        {
            m_fromValue = from;
            m_toValue = to;
            m_lerpDuration.SetValue(0);
            enabled = true;
            LerpStart?.Invoke(this, EventActionArgs.Empty);
        }

        protected override void GoToPreviousItem()
        {
            m_currentNavigationIndex--;
            Lerp(m_scrollBar.value, m_intervalDistance * m_currentNavigationIndex);
        }

        protected override void GoToNextItem()
        {
            m_currentNavigationIndex++;
            Lerp(m_scrollBar.value, m_intervalDistance * m_currentNavigationIndex);
        }

        protected override void Awake()
        {
            base.Awake();
            m_intervalDistance = 1f / m_steps;
        }

        protected override void Start()
        {
            base.Start();
            enabled = false;
        }

        private void LateUpdate()
        {
            m_lerpDuration.Update(Time.deltaTime);
            m_scrollBar.value = Mathf.Lerp(m_fromValue, m_toValue, m_lerpDuration.lerpValue);
            if (m_lerpDuration.lerpValue == 1)
            {
                enabled = false;
                LerpEnd?.Invoke(this, EventActionArgs.Empty);
            }
        }
    }
}