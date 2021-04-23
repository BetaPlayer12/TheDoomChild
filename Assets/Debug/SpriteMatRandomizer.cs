using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public class SpriteMatRandomizer : MonoBehaviour
    {
        [SerializeField, TabGroup("Sprites"), PreviewField]
        private Sprite[] m_spriteToQualify;
        [SerializeField, TabGroup("Materials")]
        private Material[] m_materialsToRandomize;

        [Button, PropertyOrder(-1)]
        private void Randomize()
        {
            var renderer = FindObjectsOfType<SpriteRenderer>();
            for (int i = 0; i < renderer.Length; i++)
            {
                if (isQualified(renderer[i]))
                {
                    renderer[i].sharedMaterial = m_materialsToRandomize[Random.Range(0, m_materialsToRandomize.Length)];
                }
            }
        }

        [Button, PropertyOrder(-1)]
        private void ResetScript()
        {
            m_spriteToQualify = new Sprite[0];
            m_materialsToRandomize = new Material[0];
        }

        private bool isQualified(SpriteRenderer renderer)
        {
            for (int i = 0; i < m_spriteToQualify.Length; i++)
            {
                if (renderer.sprite == m_spriteToQualify[i])
                {
                    return true;
                }
            }

            return false;
        }
    }

}