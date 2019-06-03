using Holysoft.Event;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public interface IStealth : IEventActionArgs
    {
        event EventAction<IStealth> StealthToggled;
        bool isStealth { get; }
        Vector2 position { get; }
    }
}
