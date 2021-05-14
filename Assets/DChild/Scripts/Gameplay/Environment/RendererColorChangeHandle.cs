using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class RendererColorChangeHandle : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] m_spriteRendererColorHandles;

        public void ApplyColor(Color color)
        {

            for (int i = 0; i < m_spriteRendererColorHandles.Length; i++)
            {
                m_spriteRendererColorHandles[i].color = color;
            }
        }
    }
}
