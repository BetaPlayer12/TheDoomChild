using UnityEngine;
using UnityEngine.Events;

namespace DChild
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SpriteList", menuName = "DChild/Sprite List")]
    public class SpriteList : ScriptableObject
    {
        [SerializeField]
        private Sprite[] m_sprites;

        public int count => m_sprites.Length;

        public Sprite GetSprite(int index) => m_sprites[index];

        public int GetIndex(Sprite sprite)
        {
            for (int i = 0; i < m_sprites.Length; i++)
            {
                if (sprite == m_sprites[i])
                    return i;
            }

            return -1;
        }
    }
}