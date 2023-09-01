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
        private Material m_closeMaterial;
        [SerializeField]
        private Sprite m_openVersion;
        [SerializeField]
        private Material m_openMaterial;

        private SpriteRenderer m_renderer;

        public override void Close(bool instant = false)
        {
            m_renderer.sprite = m_closeVersion;
            m_renderer.material = m_closeMaterial;
        }

        public override void Open(bool instant = false)
        {
            m_renderer.sprite = m_openVersion;
            m_renderer.material = m_openMaterial;
        }

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }
    }
}