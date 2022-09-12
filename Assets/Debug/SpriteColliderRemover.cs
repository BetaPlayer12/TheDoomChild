using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public class SpriteColliderRemover : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> m_toRemoveColliders;

        [Button]
        private void RemoveColliders()
        {
            var sprites = FindObjectsOfType<SpriteRenderer>();
            for (int i = 0; i < sprites.Length; i++)
            {
                if (m_toRemoveColliders.Contains(sprites[i].sprite))
                {
                    if (sprites[i].TryGetComponent(out Collider2D collider))
                    {
                        DestroyImmediate(collider);
                    }
                }
            }
        }
    }

}