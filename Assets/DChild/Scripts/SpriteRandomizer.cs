using DChild.Menu.Extras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild
{
    public class TextureRandomizer : MonoBehaviour
    {
        [SerializeField]
        private SpriteList m_list;

        public void RandomizeSprite(SpriteRenderer renderer)
        {
            var index = Random.Range(0, m_list.count);
            renderer.sprite = m_list.GetSprite(index);
        }

        public void RandomizeImage(Image renderer)
        {
            var index = Random.Range(0, m_list.count);
            renderer.sprite = m_list.GetSprite(index);
        }
    }
}