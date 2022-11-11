using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Temp
{
    public class GameEventMessage
    {
        public bool HasGameEvent { get; }
        public string EventName { get; }

        public static void SendEvent(string eventName)
        {

        }

        public static void AddListener<T>(Action<T> thing)
        {

        }

        public static void RemoveListener<T>(Action<T> thing)
        {

        }

        public static void Send()
        {

        }
    }
}
