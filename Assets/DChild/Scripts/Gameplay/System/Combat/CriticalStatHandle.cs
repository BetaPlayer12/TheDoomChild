using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class CriticalStatHandle : SerializedMonoBehaviour
    {
        [SerializeField]
        private ICappedStat m_stat;
        [SerializeField,MinValue(1)]
        private int m_valueThreshold;
        
        private void OnStatValueChanged(object sender, StatInfoEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            m_stat.ValueChanged += OnStatValueChanged;
        }

    }
}