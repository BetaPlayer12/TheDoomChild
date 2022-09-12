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
        private MaterialParameterCall m_materialParameterCall;

        public override void Close(bool instant = false)
        {
            m_renderer.sprite = m_closeVersion;
            m_materialParameterCall.SetValue(true);
        }

        public override void Open(bool instant = false)
        {
            m_renderer.sprite = m_openVersion;
            m_materialParameterCall.SetValue(false);
        }

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_materialParameterCall = GetComponent<MaterialParameterCall>();
        }
    }
}