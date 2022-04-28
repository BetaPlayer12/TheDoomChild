/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class LootChestSpriteSwapVisual : LootChestVisual
    {
        [SerializeField]
        private Sprite m_closeVersion;
        [SerializeField]
        private Sprite m_openVersion;

        private SpriteRenderer m_renderer;

        public override void Close(bool instant = false)
        {
            m_renderer.sprite = m_closeVersion;
        }

        public override void Open(bool instant = false)
        {
            m_renderer.sprite = m_openVersion;
        }

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }
    }
}