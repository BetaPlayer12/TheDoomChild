using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public struct TimeKeeper
    {
        [SerializeField]
        [MinValue(0)]
        private int m_hours;
        [SerializeField]
        private int m_minutes;
        [SerializeField]
        private int m_seconds;
        [ReadOnly]
        private float m_secondsTimer;

        public int hours { get => m_hours; set => m_hours = Mathf.Max(0, value); }

        public int minutes
        {
            get => m_minutes;
            set
            {
                m_minutes = value;
                AlignTime();
            }
        }

        public int seconds
        {
            get => m_seconds;
            set
            {
                m_seconds = value;
                AlignTime();
            }
        }


        public void Update(float deltaTime)
        {
            m_secondsTimer += deltaTime;
            if (m_secondsTimer >= 1)
            {
                var addedSeconds = Mathf.FloorToInt(m_secondsTimer);
                m_secondsTimer -= addedSeconds;
                m_seconds += addedSeconds;
                if (m_seconds >= 60)
                {
                    var addedMinutes = Mathf.FloorToInt(m_seconds / 60);
                    m_seconds = m_seconds % 60;
                    m_minutes += addedMinutes;
                    if (m_minutes >= 60)
                    {
                        var addedhours = Mathf.FloorToInt(m_minutes / 60);
                        m_minutes = m_minutes % 60;
                        m_hours += addedhours;
                    }
                }
            }
        }

        public void AlignTime()
        {
            if (m_seconds >= 60)
            {
                var addedMinutes = Mathf.FloorToInt(m_seconds / 60);
                m_seconds = m_seconds % 60;
                m_minutes += addedMinutes;

            }
            else if (m_seconds < 0)
            {
                if (m_minutes > 0)
                {
                    var positiveSeconds = Mathf.Abs(m_seconds);
                    var deductedMinutes = Mathf.FloorToInt(positiveSeconds / 60) + 1;
                    m_minutes -= deductedMinutes;
                    m_seconds = 60 - (positiveSeconds % 60);
                    if (m_seconds == 60)
                    {
                        m_seconds = 0;
                    }
                }
                else
                {
                    m_seconds = 0;
                }
            }

            if (m_minutes >= 60)
            {
                var addedhours = Mathf.FloorToInt(m_minutes / 60);
                m_minutes = m_minutes % 60;
                m_hours += addedhours;
            }
            else if (m_minutes < 0)
            {
                if (m_hours > 0)
                {
                    var positiveMinutes = Mathf.Abs(m_minutes);
                    var deductedHours = Mathf.FloorToInt(positiveMinutes / 60) + 1;
                    m_hours -= deductedHours;
                    m_minutes = 60 - (positiveMinutes % 60);
                    if (m_minutes == 60)
                    {
                        m_minutes = 0;
                    }
                }
                else
                {
                    m_minutes = 0;
                }
            }
            if (m_hours < 0)
            {
                m_hours = 0;
            }
        }
    }
}