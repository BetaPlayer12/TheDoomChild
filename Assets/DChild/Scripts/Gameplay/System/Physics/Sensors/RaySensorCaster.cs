using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class RaySensorCaster : MonoBehaviour
    {
        [SerializeField]
        [ValueDropdown("GetChildrenSensors", ExcludeExistingValuesInList = true)]
        private RaySensor[] m_sensors;

        [SerializeField]
        private bool m_useDistance;
        [SerializeField]
        [ShowIf("m_useDistance")]
        [Indent]
        private float m_castPerDistance;

        [Space]
        [SerializeField]
        private bool m_useTime;
        [SerializeField, ShowIf("m_useTime"), Indent]
        private CountdownTimer m_castPerSecond;

        private Vector3[] m_previousPosition;
        private bool[] m_hasCasted;

        private void CastViaDistance()
        {
            for (int i = 0; i < m_sensors.Length; i++)
            {
                if (Vector3.Distance(m_previousPosition[i], m_sensors[i].transform.position) >= m_castPerDistance)
                {
                    m_sensors[i].Cast();
                    m_previousPosition[i] = m_sensors[i].transform.position;
                    m_hasCasted[i] = true;
                }
            }
        }

        private void OnTimerEnd(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < m_sensors.Length; i++)
            {
                if (m_hasCasted[i])
                {
                    m_hasCasted[i] = false;
                }
                else
                {
                    m_sensors[i].Cast();
                    m_previousPosition[i] = m_sensors[i].transform.position;
                }
            }
            m_castPerSecond.Reset();
        }

        private void Start()
        {
            m_previousPosition = new Vector3[m_sensors.Length];
            m_hasCasted = new bool[m_sensors.Length];
            if (m_useTime)
            {
                m_castPerSecond.CountdownEnd += OnTimerEnd;
                m_castPerSecond.Reset();
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (m_useDistance)
            {
                CastViaDistance();
            }
            if (m_useTime)
            {
                m_castPerSecond.Tick(GameplaySystem.time.deltaTime);
            }
        }

#if UNITY_EDITOR
        public void Initialize(params RaySensor[] sensors)
        {
            m_sensors = sensors;
        }

        public void Initialize(bool useDistance, bool useTime)
        {
            m_useDistance = useDistance;
            m_useTime = useDistance;
        }

        public void InitializeDistance(float distance)
        {
            m_castPerDistance = distance;
        }

        public void InitializeTime(float time)
        {
            m_castPerSecond = new CountdownTimer(time);
        }

        private IEnumerable GetChildrenSensors()
        {
            return GetComponentsInChildren<RaySensor>();
        }
#endif
    }
}
