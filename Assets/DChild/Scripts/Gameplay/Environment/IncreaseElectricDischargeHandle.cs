using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class IncreaseElectricDischargeHandle : MonoBehaviour
    {
        [SerializeField]
        public int m_electricDischarge = 0;
        [SerializeField]
        public int m_incrementElectricDischarge;
        [SerializeField]
        public int m_electricDischargeThreshold;
        [SerializeField]
        public float m_resetElectricDischargeDelay;
        private bool m_limit = false;
        private int m_originalCharge;
        [Button]
        public void increaseCharge()
        {
            if (m_limit == false)
            {
                m_electricDischarge = m_electricDischarge + m_incrementElectricDischarge;
                if (m_electricDischarge >= m_electricDischargeThreshold)
                {

                    m_limit = true;
                    StartCoroutine(DelayCoroutine());

                }
            }

        }
        IEnumerator DelayCoroutine()
        {

            yield return new WaitForSeconds(m_resetElectricDischargeDelay);
            m_electricDischarge = m_originalCharge;
            m_limit = false;

        }
        private void Start()
        {
            m_originalCharge = m_electricDischarge;
        }
    }
}
