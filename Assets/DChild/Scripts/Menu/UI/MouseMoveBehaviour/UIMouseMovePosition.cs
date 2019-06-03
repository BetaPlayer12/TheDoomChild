using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu
{

    public class UIMouseMovePosition : MouseMovePositionBehaviour
    {
        private RectTransform m_rectTransform;

        protected override Vector2 value { get => m_rectTransform.anchoredPosition; set => m_rectTransform.anchoredPosition = value; }

        protected override void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            base.Awake();
        }
    }
}