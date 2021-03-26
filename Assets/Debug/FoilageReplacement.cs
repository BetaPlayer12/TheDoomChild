using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Gameplay.Environment
{
    public class FoilageReplacement : SerializedMonoBehaviour
    {
        public class Command
        {
            [SerializeField]
            private Sprite m_replaceWith;
            [SerializeField]
            private Material m_material;
            [SerializeField]
            private float m_reorentationOffset;
            [SerializeField]
            private bool m_updateFlip = true;

            public void ReplaceData(SpriteRenderer renderer)
            {
                renderer.sprite = m_replaceWith;
                renderer.material = m_material;
                var rotation = renderer.transform.rotation.eulerAngles;
                rotation.z += m_reorentationOffset;
                renderer.transform.rotation = Quaternion.Euler(rotation);
                if (m_updateFlip)
                {
                    var boolFlipX = renderer.flipY;
                    var boolFlipY = renderer.flipX;
                    renderer.flipY = boolFlipY;
                    renderer.flipX = boolFlipX;
                    m_updateFlip = false;
                }
            }
        }

        [SerializeField]
        private GameObject m_scope;
        [SerializeField]
        private Dictionary<Sprite, Command> m_stuffToDo;

        [Button, PropertyOrder(-1)]
        private void Execute()
        {
            SpriteRenderer[] spriteRenderers = null;
            if(m_scope == null)
            {
                spriteRenderers = FindObjectsOfType<SpriteRenderer>();
            }
            else
            {
                spriteRenderers = m_scope.GetComponentsInChildren<SpriteRenderer>();
            }

            foreach (var renderer in spriteRenderers)
            {
                if (m_stuffToDo.TryGetValue(renderer.sprite, out Command command))
                {
                    command.ReplaceData(renderer);
                }
            }
        }
    }
}