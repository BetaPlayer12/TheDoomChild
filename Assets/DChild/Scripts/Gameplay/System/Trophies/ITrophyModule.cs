using System;
using Holysoft.Event;

namespace DChild.Gameplay.Trohpies
{
    public interface ITrophyModule
    {
        event EventAction<EventActionArgs> Complete;
        ITrophyModule CreateCopy();
        bool isComplete();
        void Initialize();
    }
}