using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class IntervalDisappearingPlatform : MonoBehaviour
    {
        [SerializeField, TabGroup("Reappear")]
        private UnityEvent m_onReappear;
        [SerializeField, TabGroup("Disappear")]
        private UnityEvent m_onDisappear;
        [SerializeField]
        public float m_Delay;
        private bool active = true;
        IEnumerator DisappearDelayCoroutine()
        {

            yield return new WaitForSeconds(m_Delay);
            m_onDisappear?.Invoke();
            active = true;

        }
        IEnumerator ReappearDelayCoroutine()
        {

            yield return new WaitForSeconds(m_Delay);
            m_onReappear?.Invoke();
            active = false;

        }
        public void Update()
        {
            if (active == true)
            {
                StartCoroutine(ReappearDelayCoroutine());

            }
            if (active == false)
            {
                StartCoroutine(DisappearDelayCoroutine());

            }

        }
    }
}
