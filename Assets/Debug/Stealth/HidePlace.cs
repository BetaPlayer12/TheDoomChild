using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class HidePlace : MonoBehaviour
    {
        [SerializeField]
        private Sprite m_barelActive;
        [SerializeField]
        private bool m_isActive;

        private Sprite m_barelInactive;
        private SpriteRenderer m_renderer;

        public bool isActive => m_isActive;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponentInParent<IStealth>();
            if (player != null)
            {
                m_renderer.sprite = m_barelActive;
                m_isActive = true;
            }
            else
            {
                m_renderer.sprite = m_barelInactive;
            }
        }

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    var player = collision.GetComponentInParent<IStealth>();
        //    if (player != null)
        //    {
        //        m_renderer.sprite = m_barelInactive;
        //        m_isActive = false;
        //    }
        //}

        private void Start()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_barelInactive = m_renderer.sprite;
        }
    }
}
