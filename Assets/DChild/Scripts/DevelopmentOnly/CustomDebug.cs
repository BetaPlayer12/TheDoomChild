using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public static class CustomDebug
    {
        public enum LogType
        {
            System_Intialize,
            Scene_Load,
            Build_PostProcessScene,
            System_ArmyBattle,
            System_Combat
        }

        private const string MAIN_TAG = "DCHILD_";
        //private static Dictionary<LogType, string> m_logTypeTagPair = new Dictionary<LogType, string>()
        //{
        //    {LogType.System_Intialize,"System_Initialize"},
        //    {LogType.Scene_Load,"Scene_Load"},
        //    {LogType.Build_PostProcessScene,"Build_PostProcessScene"},
        //};

        public static void Log(LogType logType, string messsage, UnityEngine.Object reference = null) => Debug.Log(GetTag(logType) + messsage, reference);
        public static void Log(LogType logType, Func<string> messsage, UnityEngine.Object reference = null)
        {
            if (Debug.unityLogger.logEnabled)
            {
                Log(logType, messsage(), reference);
            }
        }

        public static void LogWarning(LogType logType, string messsage, UnityEngine.Object reference = null) => Debug.LogWarning(GetTag(logType) + messsage, reference);
        public static void LogError(LogType logType, string messsage, UnityEngine.Object reference = null) => Debug.LogError(GetTag(logType) + messsage, reference);

        private static string GetTag(LogType logType)
        {
            return $"[{MAIN_TAG}{logType.ToString()}]";
        }
    }

}