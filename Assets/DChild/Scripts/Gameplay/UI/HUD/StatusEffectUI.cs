using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment.UI
{
    public class StatusEffectUI : MonoBehaviour
    {
        [SerializeField]
        private StatusEffectReciever m_reciever;

        [SerializeField]
        private StatusEffectScreenFilterUI m_screenFilter;

        private void OnStatusRecieved(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            m_screenFilter.ShowFilter(eventArgs.type);
        }

        private void OnStatusEnd(object sender, StatusEffectRecieverEventArgs eventArgs)
        {
            m_screenFilter.HideFilter(eventArgs.type);
        }

        private void Awake()
        {
            m_reciever.StatusRecieved += OnStatusRecieved;
            m_reciever.StatusEnd += OnStatusEnd;

            m_screenFilter.HideFilter(StatusEffectType._COUNT);
        }
    }
}