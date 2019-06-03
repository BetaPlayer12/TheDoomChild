using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class StealthSnap : MonoBehaviour
    {
        private Vector3 m_initialPosition;
        private Rigidbody2D m_rigidbody;

        private IStealth m_stealth;
        private bool m_isHiding;

        private void Update()
        {
            if (m_stealth.isStealth)
            {
                if (m_isHiding)
                {
                    if (ScanNearestObject() != null)
                    {
                        m_rigidbody.transform.position = ScanNearestObject().transform.position;
                    }
                }
                else
                {
                    m_initialPosition = m_rigidbody.transform.position;
                    m_isHiding = true;
                }
            }

            else
            {
                if (m_isHiding)
                {
                    m_rigidbody.transform.position = m_initialPosition;
                    m_isHiding = false;
                }
            }
        }

        private HidePlace ScanNearestObject()
        {
            var objects = FindObjectsOfType<HidePlace>();
            if (objects[0].isActive)
            {
                return objects[0];
            }
            else
            {
                return null;
            }
        }

        private void Start()
        {
            m_stealth = GetComponent<IStealth>();
            m_rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
