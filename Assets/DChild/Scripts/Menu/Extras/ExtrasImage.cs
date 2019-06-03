using System;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Extras
{
    public struct ExtrasImageEventArgs : IEventActionArgs
    {
        public ExtrasImageEventArgs(int id, Sprite sprite) : this()
        {
            this.id = id;
            this.sprite = sprite;
        }

        public int id { get; }
        public Sprite sprite { get;}
    }

    public class ExtrasImage : ExtrasItem
    {
        [SerializeField]
        private Image m_image;
        [SerializeField]
        private Sprite m_sprite;

        public event EventAction<ExtrasImageEventArgs> ImageSelected;

        public void SetSprite(Sprite sprite)
        {
            m_sprite = sprite;
            m_image.sprite = sprite;
        }

        public void Select()
        {
            ImageSelected?.Invoke(this, new ExtrasImageEventArgs(m_id,m_sprite));
        }

        private void OnValidate()
        {
            if (m_image == null)
            {
                m_image = GetComponent<Image>();
            }
            else if (m_sprite != null)
            {
                m_image.sprite = m_sprite;
            }
        }
    }
}