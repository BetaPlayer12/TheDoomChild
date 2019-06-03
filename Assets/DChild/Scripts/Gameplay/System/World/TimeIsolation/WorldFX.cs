using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    public class WorldFX : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private FXObjects m_fxOBjects;

        private void OnEnable()
        {
            if (m_fxOBjects != null)
            {
                GameplaySystem.world.Register(m_fxOBjects);
            }
        }

        private void OnDisable()
        {
            if (m_fxOBjects != null)
            {
                GameplaySystem.world.Unregister(m_fxOBjects);
            }
        }

        private void OnValidate()
        {
            var particlesSystems = GetComponentsInChildren<ParticleSystem>();
            if (particlesSystems == null)
            {
                m_fxOBjects = null;
            }
            else
            {
                m_fxOBjects = new FXObjects(GetComponentsInChildren<ParticleSystem>());
            }
        }
    }
}