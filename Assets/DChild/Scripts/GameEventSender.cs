using Doozy.Engine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class GameEventSender : MonoBehaviour
    {
        [SerializeField]
        private string m_event;

        [Button]
        public void SendSavedEvent() => GameEventMessage.SendEvent(m_event);
        public void SendEvent(string toSend) => GameEventMessage.SendEvent(toSend);
    }
}