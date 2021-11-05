using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class CursorTrails : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem m_trail;

        private void OnDisable()
        {
            m_trail.Clear(true);
        }
    }

}