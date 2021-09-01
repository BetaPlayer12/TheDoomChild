using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class PlatformExtenderConfigurator : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        private enum SideConfiguration
        {
            CenterOnly,
            OneSide,

            AllSide
        }

        private const string HASSIDECONFIGURATION_CONDITION = "@m_sideConfiguration != SideConfiguration.CenterOnly";
        private const string ONESIDECONFIGURATION_CONDITION = "@m_sideConfiguration == SideConfiguration.OneSide";
        private const string AllSIDECONFIGURATION_CONDITION = "@m_sideConfiguration == SideConfiguration.AllSide";

        [SerializeField]
        private Sprite m_centerSprite;
        [SerializeField]
        private Sprite m_sideSprite;
        [SerializeField]
        private SideConfiguration m_sideConfiguration;
        [SerializeField, ShowIf(HASSIDECONFIGURATION_CONDITION), Indent]
        private bool m_doesSideSpriteFacesRight = true;
        [SerializeField, ShowIf(HASSIDECONFIGURATION_CONDITION), Indent]
        private bool m_flipX;

        [SerializeField, ShowIf(ONESIDECONFIGURATION_CONDITION), Indent]
        private float m_sideOffsetX;
        [SerializeField, ShowIf(ONESIDECONFIGURATION_CONDITION), Indent]
        private float m_sideEdgeModifier;

        [SerializeField, ShowIf(AllSIDECONFIGURATION_CONDITION), Indent]
        private Vector2 m_sideOffset;
        [SerializeField, ShowIf(AllSIDECONFIGURATION_CONDITION), Indent]
        private float m_sideLength;

        [SerializeField]
        private float m_platformLength;

        [SerializeField, HideInInspector]
        private Transform m_rightSide;
        [SerializeField, HideInInspector]
        private Transform m_leftSide;

        private void OnValidate()
        {
            if (PrefabUtility.IsPartOfPrefabAsset(this))
                return;

            var selfSprite = GetComponent<SpriteRenderer>();
            selfSprite.drawMode = SpriteDrawMode.Tiled;
            var size = selfSprite.size;
            size.x = m_platformLength;
            selfSprite.size = size;
            selfSprite.sortingLayerName = "PlayableGround";
            selfSprite.sortingOrder = 1;


            var sidePositionOffset = m_sideOffset;
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

            var boxCollider = GetComponent<BoxCollider2D>();
            var boxColliderSize = boxCollider.size;
            boxColliderSize.x = m_platformLength;

            var boxColliderOffset = boxCollider.offset;
            boxColliderOffset.x = 0;

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
                    boxColliderSize.x -= (m_sideOffsetX / 2) + m_sideEdgeModifier;
                    boxColliderOffset.x = ((m_platformLength + m_sideOffsetX) / 2) - (m_sideEdgeModifier / 2);
                    boxColliderOffset.x *= (m_flipX ? -1 : 1);
                    break;
                case SideConfiguration.AllSide:
                    ValidateSprite(selfSprite, m_centerSprite);
                    selfSprite.flipX = false;

                    ValidateObject(ref m_rightSide, "RightPlatformEdge");
                    sidePositionOffset.x += m_platformLength / 2;
                    sidePositionOffset.x *= (m_doesSideSpriteFacesRight ? -1 : 1);
                    m_rightSide.localPosition = sidePositionOffset;
                    rightRenderer = m_rightSide.GetComponent<SpriteRenderer>();
                    ValidateSprite(rightRenderer, m_sideSprite);
                    rightRenderer.flipX = !m_doesSideSpriteFacesRight;
                    rightRenderer.sortingLayerName = "PlayableGround";

                    ValidateObject(ref m_leftSide, "LeftPlatformEdge");
                    sidePositionOffset.x *= -1;
                    m_leftSide.localPosition = sidePositionOffset;
                    leftRenderer = m_leftSide.GetComponent<SpriteRenderer>();
                    ValidateSprite(leftRenderer, m_sideSprite);
                    leftRenderer.flipX = m_doesSideSpriteFacesRight;
                    leftRenderer.sortingLayerName = "PlayableGround";
                    boxColliderSize.x += m_sideLength;
                    break;
            }

            boxCollider.size = boxColliderSize;
            boxCollider.offset = boxColliderOffset;
        }

        private void ValidateObject(ref Transform GameObject, string name)
        {
            if (GameObject == null)
            {
                GameObject = (new GameObject(name)).transform;
                GameObject.gameObject.AddComponent<SpriteRenderer>();
                GameObject.SetParent(transform);
                GameObject.transform.localScale = Vector3.one;
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