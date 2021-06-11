
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment.Interractables
{
    public class SpikeTrap : MonoBehaviour
    {
        [SerializeField, TabGroup("Onstick")]
        private UnityEvent m_onStick;
        [SerializeField, TabGroup("Onleave")]
        private UnityEvent m_onLeave;
        [SerializeField]
        public float m_SpikeDelay;
        [SerializeField]
        private Transform m_trapSensor;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Sensor")
            {
                StartCoroutine(DelayCoroutineStick());

            }
        }



        IEnumerator DelayCoroutineStick()
        {
            m_trapSensor.GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(m_SpikeDelay);
            m_onStick?.Invoke();
            StartCoroutine(DelayCoroutineLeave());

        }
        IEnumerator DelayCoroutineLeave()
        {
            CancelInvoke();
            yield return new WaitForSeconds(m_SpikeDelay);
            m_onLeave?.Invoke();
            m_trapSensor.GetComponent<Collider2D>().enabled = true;

        }

    }
}
