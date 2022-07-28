using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public class PostuleShineHandle : MonoBehaviour
    {
        [SerializeField]
        private string m_lightSourceProperty;
        [SerializeField,ValueDropdown("GetAllPostuleShine",IsUniqueList =true)]
        private PostuleBulb[] m_bulb;

        private MaterialPropertyBlock m_materialPropertyBlock;

        private IEnumerable GetAllPostuleShine()
        {
            var bulbs = FindObjectsOfType<PostuleBulb>();
            var options = new ValueDropdownList<PostuleBulb>();
            for (int i = 0; i < bulbs.Length; i++)
            {
                var bulb = bulbs[i];
                options.Add($"All/{GetHeirarchyPath(bulb.transform)}", bulb);
            }
            return options;
        }

        private string GetHeirarchyPath(Transform transform)
        {
            if(transform.parent == null)
            {
                return transform.name;
            }
            else
            {
                return $"{GetHeirarchyPath(transform.parent)}/{transform.name}";
            }
        }

        private void Start()
        {
            m_materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void LateUpdate()
        {
            var playerPosition = GameplaySystem.playerManager.player.character.centerMass.position;
            for (int i = 0; i < m_bulb.Length; i++)
            {
                var renderer = m_bulb[i].spriteRenderer;
                renderer.GetPropertyBlock(m_materialPropertyBlock);
                m_materialPropertyBlock.SetVector(m_lightSourceProperty, playerPosition);
                renderer.SetPropertyBlock(m_materialPropertyBlock);
            }
        }
    }
}