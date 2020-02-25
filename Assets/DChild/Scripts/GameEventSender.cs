using Doozy.Engine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

namespace DChild
{
    public class GameEventSender : MonoBehaviour
    {
        private enum System
        {
            Doozy,
            MasterAudio
        }

        [SerializeField, HideInInspector]
        private string m_event;
        [SerializeField, OnValueChanged("OnSystemChange")]
        private System m_systemToSendTo;

        [Button]
        public void SendSavedEvent()
        {
            switch (m_systemToSendTo)
            {
                case System.Doozy:
                    GameEventMessage.SendEvent(m_event);
                    break;
                case System.MasterAudio:
                    MasterAudio.FireCustomEvent(m_event, transform);
                    break;
            }
        }

        public void SendAsDozzyEvent(string toSend) => GameEventMessage.SendEvent(toSend);
        public void SendAsMasterAudioEvent(string toSend) => MasterAudio.FireCustomEvent(toSend, transform);

#if UNITY_EDITOR
        [SerializeField, OnValueChanged("OnValueChange"),
         ShowIf("@m_systemToSendTo == System.Doozy"), LabelText("Event")]
        private string m_doozyEvent;
        [SerializeField, MasterCustomEvent, OnValueChanged("OnValueChange"),
         ShowIf("@m_systemToSendTo == System.MasterAudio"), LabelText("Event")]
        private string m_masterAudioEvent;

        private void OnSystemChange()
        {
            switch (m_systemToSendTo)
            {
                case System.Doozy:
                    m_event = m_doozyEvent;
                    break;
                case System.MasterAudio:
                    m_event = m_masterAudioEvent;
                    break;
            }
        }

        private void OnValueChange()
        {
            switch (m_systemToSendTo)
            {
                case System.Doozy:
                    m_event = m_doozyEvent;
                    break;
                case System.MasterAudio:
                    m_event = m_masterAudioEvent;
                    break;
            }
        }
#endif
    }
}