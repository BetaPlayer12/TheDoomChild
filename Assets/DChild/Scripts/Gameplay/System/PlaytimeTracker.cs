using Holysoft.Collections;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class PlaytimeTracker : MonoBehaviour
    {
        private float m_currentTime;
#if UNITY_EDITOR
        [SerializeField, DisplayAsString]
        private string m_time;
#endif
        private void OnSave(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            eventArgs.slot.UpdateDuration(m_currentTime);
        }

        private void OnLoad(object sender, CampaignSlotUpdateEventArgs eventArgs) => m_currentTime = eventArgs.slot.duration;

        private void Awake()
        {
            m_currentTime = GameplaySystem.campaignSerializer.slot.duration;
            GameplaySystem.campaignSerializer.PostDeserialization += OnLoad;
            GameplaySystem.campaignSerializer.PreSerialization += OnSave;
        }

        void LateUpdate()
        {
            m_currentTime += Time.unscaledDeltaTime;
#if UNITY_EDITOR
            var span = TimeSpan.FromSeconds(m_currentTime);
            m_time = $"{((int)span.TotalHours).ToString("D2")}:{span.Minutes.ToString("D2")}:{span.Seconds.ToString("D2")}.{span.Milliseconds.ToString("D2")}";
#endif
        }
    }

}