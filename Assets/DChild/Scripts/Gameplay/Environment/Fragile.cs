using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class Fragile : MonoBehaviour
    {
        [SerializeField] [MinValue(0)]
        private float m_breakForce;
        private IFragileObject m_fragileObject;

        private void Awake()
        {
            m_fragileObject = GetComponent<IFragileObject>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.relativeVelocity.magnitude >= m_breakForce)
            {
                m_fragileObject.Break();
            }
        }
    }
}