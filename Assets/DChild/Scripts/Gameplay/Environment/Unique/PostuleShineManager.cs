﻿using Sirenix.OdinInspector;
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
            return FindObjectsOfType<PostuleBulb>();
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