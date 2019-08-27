using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [DisallowMultipleComponent]
    public class Surface : MonoBehaviour
    {
        [SerializeField]
        private SurfaceData m_data;

        public SurfaceData data => m_data;
    }
}
