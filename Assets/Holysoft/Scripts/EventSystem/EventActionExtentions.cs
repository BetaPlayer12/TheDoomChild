using Holysoft.Event.Collections;
using UnityEngine;

namespace Holysoft.Event
{
    public static class EventActionExtentions
    {
        #region Safe
        //private static T RaiseDelegate<T>(object sender, Delegate currentHandler, T eventArgs, string message) where T : struct, IEventActionArgs
        //{
        //    EventAction<T> currentSubscriber = (EventAction<T>)currentHandler;
        //    try
        //    {
        //        return currentSubscriber(sender, eventArgs);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError(message);
        //        Debug.LogError(ex.Message);
        //        return eventArgs;
        //    }
        //}

        public static void Raise<T>(this MonoBehaviour monobehaviour, EventAction<T> handler, T eventArgs) where T :  IEventActionArgs
        {
            handler?.Invoke(monobehaviour, eventArgs);
        }

        public static void Raise<T>(this UnityEngine.Object ueObject, EventAction<T> handler, T eventArgs) where T : IEventActionArgs
        {
            handler?.Invoke(ueObject, eventArgs);
        }

        public static void Raise<T>(this System.Object sObject, EventAction<T> handler, T eventArgs) where T :  IEventActionArgs
        {
            handler?.Invoke(sObject, eventArgs);
        }
        #endregion Safe
    }
}