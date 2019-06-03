using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public sealed class UIScrollViewContentCreator : MonoBehaviour
    {
#if UNITY_EDITOR
        [Button("Update Content")]
        private void UpdateContent()
        {
            var factory = GetComponent<ReferenceFactory>();
            var listeners = GetComponents<IEventListener>();
            for (int i = 0; i < listeners.Length; i++)
            {
                listeners[i].SubscribeToEvents();
            }
            factory.RestartProduction();
            for (int i = 0; i < listeners.Length; i++)
            {
                listeners[i].UnsubscribeToEvents();
            }
        }
#endif
    }
}