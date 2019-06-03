using Holysoft;
using Holysoft.Event;
using System;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public struct StatusRecieverEventArgs : IEventActionArgs
    {
        public StatusRecieverEventArgs(IStatusReciever reciever) : this()
        {
            this.reciever = reciever;
        }

       public IStatusReciever reciever {get;}
    }

    public interface IStatusReciever
    {
        Transform transform { get; }
        IStatusEffectState statusEffectState { get; }
        IStatusResistance statusResistance { get; }
        T GetComponentInChildren<T>();
        Component GetComponentInChildren(Type type);
        event EventAction<StatusRecieverEventArgs> ReceiverDestroyed;
    }
}