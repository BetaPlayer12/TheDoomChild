using DChild.Menu.Extras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField]
        private SpriteList m_list;

        private void RandomizeSprite(SpriteRenderer renderer)
        {
            var index = Random.Range(0, m_list.count);
            renderer.sprite = m_list.GetSprite(index);
        }
    }
}