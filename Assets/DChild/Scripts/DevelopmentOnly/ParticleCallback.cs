using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay
{
    public class ParticleCallback : MonoBehaviour
    {
        public event EventAction<EventActionArgs> CallBack;

        private void OnParticleSystemStopped()
        {
            CallBack?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnValidate()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particleSystems.Length; i++)
            {
                var main = particleSystems[i].main;
#if UNITY_EDITOR
                if (main.stopAction != ParticleSystemStopAction.Callback)
                {
                    main.stopAction = ParticleSystemStopAction.Callback;
                }
#endif
            }
        }
    }
}