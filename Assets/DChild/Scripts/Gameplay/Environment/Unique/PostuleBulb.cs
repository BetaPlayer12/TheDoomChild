using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public class PostuleBulb : MonoBehaviour
    {
        private SpriteRenderer m_renderer;
        public SpriteRenderer spriteRenderer => m_renderer;

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }
    }
}