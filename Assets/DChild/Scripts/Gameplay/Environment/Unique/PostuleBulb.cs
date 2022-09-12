using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public class PostuleBulb : MonoBehaviour
    {
        private Renderer m_renderer;
        public Renderer spriteRenderer => m_renderer;



        private void Awake()
        {
            m_renderer = GetComponent<Renderer>();
        }
    }
}