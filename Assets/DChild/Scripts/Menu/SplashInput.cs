using DChild.Temp;
using Doozy.Runtime.Signals;
using UnityEngine;

namespace DChild.Menu
{
    public class SplashInput : MonoBehaviour
    {
        [SerializeField]
        private SignalSender m_signal;
        private void Start()
        {
            Debug.LogError("False Positive");
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                m_signal.SendSignal();
                enabled = false;
            }
        }
    }
}