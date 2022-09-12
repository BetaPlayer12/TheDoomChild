using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace DChild.Gameplay.Environment
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MultiSprite : MonoBehaviour
    {
        [SerializeField, OnInspectorGUI("DrawPreview")]
        private SpriteList m_data;
        [SerializeField, HideInInspector]
        private int m_index = 0;
#if UNITY_EDITOR
        private void DrawPreview()
        {
            if (m_data != null)
            {
                SirenixEditorGUI.BeginHorizontalPropertyLayout(new GUIContent());
                SirenixEditorGUI.IndentSpace();
                if (SirenixEditorGUI.IconButton(ConvertToTexture2D(m_data.GetSprite((int)Mathf.Repeat(m_index - 1, m_data.count))), 50, 50))
                {
                    ChangeModel(-1);
                }
                SirenixEditorGUI.IconButton(ConvertToTexture2D(m_data.GetSprite(m_index)), 100, 100);
                if (SirenixEditorGUI.IconButton(ConvertToTexture2D(m_data.GetSprite((int)Mathf.Repeat(m_index + 1, m_data.count))), 50, 50))
                {
                    ChangeModel(1);
                }
                SirenixEditorGUI.EndHorizontalPropertyLayout();
            }

            void ChangeModel(int moveIndexDistance)
            {
                m_index = (int)Mathf.Repeat(m_index + moveIndexDistance, m_data.count);
                gameObject.GetComponent<SpriteRenderer>().sprite = m_data.GetSprite(m_index);
            }

            Texture2D ConvertToTexture2D(Sprite sprite)
            {
                if (sprite.rect.width != sprite.texture.width)
                {
                    Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                    Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                                 (int)sprite.textureRect.y,
                                                                 (int)sprite.textureRect.width,
                                                                 (int)sprite.textureRect.height);
                    newText.SetPixels(newColors);
                    newText.Apply();
                    return newText;
                }
                else
                    return sprite.texture;
            }
        }
#endif
    }
}