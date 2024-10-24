﻿using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    [AddComponentMenu("DChild/Gameplay/Combat/Magic")]
    public class Magic : MonoBehaviour, ICappedStat
    {
        [SerializeField, MinValue(0), OnValueChanged("SendMaxValue")]
        private int m_maxPoints;

        [ShowInInspector, HideInEditorMode, OnValueChanged("SendValueEvent"), MinValue(0),MaxValue("$m_maxPoints")]
        private int m_currentPoints;
        [ShowInInspector, ReadOnly, ProgressBar(0f, 1f)]
        protected float m_percentage;

        public event EventAction<StatInfoEventArgs> ValueChanged;
        public event EventAction<StatInfoEventArgs> MaxValueChanged;

        public bool isFull => m_currentPoints == m_maxPoints;
        public bool isEmpty => m_currentPoints <= 0f;
        public int currentValue => m_currentPoints;
        public int maxValue => m_maxPoints;

        public void ReduceCurrentValue(int points)
        {
            m_currentPoints = Mathf.Max(0, m_currentPoints - points);
            m_percentage = (float)m_currentPoints / m_maxPoints;
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentPoints, m_maxPoints));
        }

        public void AddCurrentValue(int points)
        {
            m_currentPoints = Mathf.Min(m_maxPoints, m_currentPoints + points);
            m_percentage = (float)m_currentPoints / m_maxPoints;
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentPoints, m_maxPoints));
        }

        public void ResetValueToMax()
        {
            m_currentPoints = m_maxPoints;
            m_percentage = 1;
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentPoints, m_maxPoints));
        }

        public void SetMaxValue(int value)
        {
            m_maxPoints = value;
            m_currentPoints = Mathf.Min(m_maxPoints, m_currentPoints);
            MaxValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentPoints, m_maxPoints));
        }

#if UNITY_EDITOR
        protected void SendValueEvent()
        {
            m_percentage = (float)m_currentPoints / maxValue;
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentPoints, maxValue));
        }

        protected void SendMaxValue()
        {
            m_percentage = (float)m_currentPoints / maxValue;
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentPoints, maxValue));
            MaxValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentPoints, maxValue));
        }
#endif
    }
}