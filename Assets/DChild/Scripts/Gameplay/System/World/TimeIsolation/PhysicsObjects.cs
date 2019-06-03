using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    [System.Serializable]
    public class PhysicsObjects : IPhysicObjects
    {
        [SerializeField]
        private Rigidbody2D[] m_rigidbody2Ds;
        [SerializeField]
        private PhysicsTimeObjectInfo[] m_physicsTimeObjectInfo;

        public PhysicsObjects(Rigidbody2D[] rigidbody2Ds)
        {
            if (rigidbody2Ds == null)
            {
                m_rigidbody2Ds = null;
                m_physicsTimeObjectInfo = null;
            }
            else
            {
                m_rigidbody2Ds = rigidbody2Ds;
                m_physicsTimeObjectInfo = new PhysicsTimeObjectInfo[m_rigidbody2Ds.Length];
                for (int i = 0; i < m_physicsTimeObjectInfo.Length; i++)
                {
                    m_physicsTimeObjectInfo[i] = new PhysicsTimeObjectInfo(m_rigidbody2Ds[i]);
                }
            }
        }

        public void CalculateActualVelocity(float timeScale)
        {
            for (int i = 0; i < m_rigidbody2Ds.Length; i++)
            {
                m_physicsTimeObjectInfo[i].CalculateActualVelocity(m_rigidbody2Ds[i], timeScale);
            }
        }

        public void AlignTime(float timeScale)
        {
            for (int i = 0; i < m_rigidbody2Ds.Length; i++)
            {
                m_physicsTimeObjectInfo[i].AlignTime(m_rigidbody2Ds[i], timeScale);
            }
        }

        public void Revert()
        {
            for (int i = 0; i < m_rigidbody2Ds.Length; i++)
            {
                m_physicsTimeObjectInfo[i].Revert(m_rigidbody2Ds[i]);
            }
        }
    }
}