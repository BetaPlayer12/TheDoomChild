using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DChildDebug
{
    public class SpriteTransfer : MonoBehaviour
    {
        [SerializeField, AssetSelector]
        private Sprite[] m_properReference;
        [SerializeField]
        private string m_prefix;

        [Button]
        private void BeginTransfer()
        {
            var sprites = FindObjectsOfType<SpriteRenderer>();
            for (int i = 0; i < sprites.Length; i++)
            {
                var renderer = sprites[i];
                var sprite = renderer.sprite;
                if (sprite != null)
                {
                    if (sprite.name.StartsWith(m_prefix) == false)
                    {
                        var supposedName = m_prefix + sprite.name;
                        var replacement = GetSprite(supposedName);
                        if (replacement)
                        {
                            renderer.sprite = replacement;
                            Debug.Log($"Transfer Success");
                        }
                        else
                        {
                            Debug.Log($"Cant Find Replacement For = {sprite.name}");
                        }
                    }
                }
                else
                {
                    Debug.Log($"Sprite Missing");
                }
            }
        }

        private Sprite GetSprite(string name)
        {
            for (int i = 0; i < m_properReference.Length; i++)
            {
                if (m_properReference[i].name == name)
                {
                    return m_properReference[i];
                }
            }
            return null;
        }
    }

}