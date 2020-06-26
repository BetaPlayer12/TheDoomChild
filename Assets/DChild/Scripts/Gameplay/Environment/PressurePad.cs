using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class PressurePad : MonoBehaviour
    {
        [SerializeField, MinValue(0.1f)]
        private float m_requiredMass;

        private float m_currentMass;
        private bool m_hasReachedRequiredMass;

        private void ReactOnChangeOnMass()
        {
            if(m_currentMass >= m_requiredMass)
            {
                if(m_hasReachedRequiredMass == false)
                {

                    m_hasReachedRequiredMass = true;
                }
            }
            else
            {
                if (m_hasReachedRequiredMass)
                {

                    m_hasReachedRequiredMass = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out Rigidbody2D rigidbody))
            {
                m_currentMass += rigidbody.mass;
                ReactOnChangeOnMass();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out Rigidbody2D rigidbody))
            {
                m_currentMass += rigidbody.mass;
                ReactOnChangeOnMass();
            }
        }
    }

}