using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class EventModuleConnector : MonoBehaviour
    {
        private void Start()
        {
            var modules = GetComponentsInChildren<IEventModule>();
            for (int i = 0; i < modules.Length; i++)
            {
                modules[i].ConnectEvents();
            }
        }
    }
}