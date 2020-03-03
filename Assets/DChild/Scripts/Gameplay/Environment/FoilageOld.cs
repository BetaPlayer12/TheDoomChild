using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class FoilageOld : MonoBehaviour
    {
        private Renderer[] m_renderers;

        private MaterialPropertyBlock m_propertyBlock;
        private bool m_isMoved;
        private bool m_movedNegatively;
        private static string property => "Vector1_BendDirection";

        private void Awake()
        {
            m_renderers = GetComponentsInChildren<Renderer>();
        }

        private void LateUpdate()
        {
            if (m_isMoved)
            {
                bool isSleeping = true;
                for (int i = 0; i < m_renderers.Length; i++)
                {
                    m_renderers[i].GetPropertyBlock(m_propertyBlock);
                    var direction = m_propertyBlock.GetFloat(property);
                    if (direction != 0)
                    {
                        var sign = Mathf.Sign(direction);
                        direction -= sign * GameplaySystem.time.deltaTime;
                        direction = Mathf.Clamp01(direction) * sign;
                        m_propertyBlock.SetFloat(property, direction);
                        m_renderers[i].SetPropertyBlock(m_propertyBlock);
                        isSleeping = false;
                    }
                }

                if (isSleeping)
                {
                    m_isMoved = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out Character character))
            {
                int direction = 0;
                if (character.centerMass.position.x < transform.position.x)
                {
                    direction = -1;
                    m_movedNegatively = true;
                }
                else
                {
                    direction = 1;
                    m_movedNegatively = false;
                }
                for (int i = 0; i < m_renderers.Length; i++)
                {
                    m_renderers[i].GetPropertyBlock(m_propertyBlock);
                    m_propertyBlock.SetFloat(property, direction);
                    m_renderers[i].SetPropertyBlock(m_propertyBlock);
                }
                m_isMoved = true;
            }
        }
    }
}