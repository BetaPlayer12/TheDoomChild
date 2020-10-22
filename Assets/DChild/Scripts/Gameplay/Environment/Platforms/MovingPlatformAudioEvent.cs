using DarkTonic.MasterAudio;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class MovingPlatformAudioEvent : MonoBehaviour
    {
        [SerializeField,Tooltip("Event is \"<Prefix> Start\" and \"<Prefix> Stop\"")]
        private string m_eventPrefix;

        private void OnStart(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            MasterAudio.FireCustomEvent(m_eventPrefix + " Start", transform);
        }

        private void OnStop(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            MasterAudio.FireCustomEvent(m_eventPrefix + " Stop", transform);
        }

        private void Awake()
        {
            var platform = GetComponent<MovingPlatform>();
            platform.DestinationReached += OnStop;
            platform.DestinationChanged += OnStart;
        }
    }
}




