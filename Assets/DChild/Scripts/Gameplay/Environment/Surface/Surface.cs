using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [DisallowMultipleComponent]
    [AddComponentMenu("DChild/Gameplay/Environment/Surface")]
    public class Surface : MonoBehaviour
    {
        [SerializeField]
        private SurfaceData m_data;

        public SurfaceData data => m_data;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var contact  = collision.contacts[0];
        }
    }
}
