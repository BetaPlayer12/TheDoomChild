using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class PlatformExtenderConfigurator : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        private enum SideConfiguration
        {
            CenterOnly,
            OneSide,
            OneSideWithCenter,
            AllSide
        }

        [SerializeField]
        private Sprite m_centerSprite;
        [SerializeField]
        private Sprite m_sideSprite;
        [SerializeField]
        private SideConfiguration m_sideConfiguration;
        [SerializeField]
        private bool m_flipX;
        [SerializeField]
        private float m_length;
        [SerializeField]
        private Vector2 m_sideOffset;

        [SerializeField, HideInInspector]
        private Transform m_rightSide;
        [SerializeField, HideInInspector]
        private Transform m_leftSide;

        private void OnValidate()
        {
            var selfSprite = GetComponent<SpriteRenderer>();
            selfSprite.drawMode = SpriteDrawMode.Tiled;
            var size = selfSprite.size;
            size.x = m_length;
            selfSprite.size = size;


            var offset = m_sideOffset;
            SpriteRenderer rightRenderer = null;
            if (m_rightSide)
            {
                rightRenderer = m_rightSide.GetComponent<SpriteRenderer>();
            }
            SpriteRenderer leftRenderer = null;
            if (m_leftSide)
            {
                leftRenderer = m_leftSide.GetComponent<SpriteRenderer>();
            }

            switch (m_sideConfiguration)
            {
                case SideConfiguration.CenterOnly:
                    ValidateSprite(selfSprite, m_centerSprite);
                    ValidateDestruction(ref m_rightSide);
                    ValidateDestruction(ref m_leftSide);
                    selfSprite.flipX = false;
                    break;
                case SideConfiguration.OneSide:
                    ValidateSprite(selfSprite, m_sideSprite);
                    ValidateDestruction(ref m_rightSide);
                    ValidateDestruction(ref m_leftSide);
                    selfSprite.flipX = m_flipX;
                    break;
                case SideConfiguration.OneSideWithCenter:
                    ValidateSprite(selfSprite, m_centerSprite);

                    ValidateObject(ref m_rightSide, "RightPlatformEdge");
                    ValidateDestruction(ref m_leftSide);
                    offset.x *= (m_flipX ? -1 : 1);
                    m_rightSide.localPosition = offset;
                    rightRenderer = m_rightSide.GetComponent<SpriteRenderer>();
                    ValidateSprite(rightRenderer, m_sideSprite);
                    rightRenderer.flipX = m_flipX;
                    break;
                case SideConfiguration.AllSide:
                    ValidateSprite(selfSprite, m_centerSprite);
                    selfSprite.flipX = false;

                    ValidateObject(ref m_rightSide, "RightPlatformEdge");
                    m_rightSide.localPosition = offset;
                    rightRenderer = m_rightSide.GetComponent<SpriteRenderer>();
                    ValidateSprite(rightRenderer, m_sideSprite);
                    rightRenderer.flipX = false;

                    ValidateObject(ref m_leftSide, "LeftPlatformEdge");
                    offset.x *= -1;
                    m_leftSide.localPosition = offset;
                    leftRenderer = m_rightSide.GetComponent<SpriteRenderer>();
                    ValidateSprite(leftRenderer, m_sideSprite);
                    leftRenderer.flipX = false;
                    break;
            }
        }

        private void ValidateObject(ref Transform GameObject, string name)
        {
            if (GameObject == null)
            {
                GameObject = (new GameObject(name)).transform;
                GameObject.gameObject.AddComponent<SpriteRenderer>();
                GameObject.SetParent(transform);
            }
        }

        private void ValidateDestruction(ref Transform GameObject)
        {
            if (GameObject)
            {
                DestroyImmediate(GameObject.gameObject);
            }
        }

        private void ValidateSprite(SpriteRenderer spriteRenderer, Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            var size = spriteRenderer.size;
            size.y = sprite.texture.height / 100f;
            spriteRenderer.size = size;
        }
#endif
    }
}