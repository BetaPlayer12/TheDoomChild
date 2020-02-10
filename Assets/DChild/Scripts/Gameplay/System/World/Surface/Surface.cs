using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class Surface : MonoBehaviour
    {
        [SerializeField]
        private SurfaceType m_type;

        public SurfaceType type => m_type;

        private void OnValidate()
        {
            gameObject.tag = "SolidPlatform";
        }
    }
}