using UnityEngine;

namespace DChild.Menu.Extras
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SpriteList", menuName = "DChild/Extras/Sprite List")]
    public class SpriteList : ScriptableObject
    {
        [SerializeField]
        private Sprite[] m_sprites;

        public int count => m_sprites.Length;

        public Sprite GetSprite(int index) => m_sprites[index];
    }
}