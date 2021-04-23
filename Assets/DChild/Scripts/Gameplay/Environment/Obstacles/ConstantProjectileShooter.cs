using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class ConstantProjectileShooter : MonoBehaviour
    {
        [System.Serializable]
        private class ShooterInfo
        {
            [SerializeField]
            private ProjectileShooter m_shooter;
            [SerializeField]
            private float m_startDelay;
            [SerializeField, Tooltip("Adds Up With FireDelay")]
            private float[] m_fireInterval;
            private int m_fireIntervalIndex = 0;

            public float startDelay => m_startDelay;
            public float fireInterval
            {
                get
                {
                    var result = m_fireInterval[m_fireIntervalIndex] + m_shooter.fireDelay;
                    m_fireIntervalIndex = (int)Mathf.Repeat(m_fireIntervalIndex + 1, m_fireInterval.Length);
                    return result;
                }
            }

            public void Shoot() => m_shooter.Shoot();
        }

        [SerializeField, TableList(AlwaysExpanded = true)]
        private ShooterInfo[] m_infos;
        private float[] m_delayTimer;
        private float[] m_shotTimer;

        private void Awake()
        {
            var infoLenght = m_infos.Length;
            m_delayTimer = new float[infoLenght];
            for (int i = 0; i < infoLenght; i++)
            {
                m_delayTimer[i] = m_infos[i].startDelay;
            }
            m_shotTimer = new float[infoLenght];
        }

        private void Update()
        {
            for (int i = 0; i < m_infos.Length; i++)
            {
                if (m_delayTimer[i] <= 0)
                {
                    if (m_shotTimer[i] <= 0)
                    {
                        m_shotTimer[i] = m_infos[i].fireInterval;
                        m_infos[i].Shoot();
                    }
                    else
                    {
                        m_shotTimer[i] -= GameplaySystem.time.deltaTime;
                    }
                }
                else
                {
                    m_delayTimer[i] -= GameplaySystem.time.deltaTime;
                }
            }
        }
    }
}
