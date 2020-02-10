using UnityEngine;

namespace DChild
{
    public class SpriteUtility : MonoBehaviour
    {
        private SpriteRenderer m_renderer;
        private Sprite m_savedSprite;

        public void SaveCurrentSprite()
        {
            m_savedSprite =  m_renderer.sprite;
        }

        public void LoadSavedSprite()
        {
            m_renderer.sprite = m_savedSprite;
        }

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }
    }
}