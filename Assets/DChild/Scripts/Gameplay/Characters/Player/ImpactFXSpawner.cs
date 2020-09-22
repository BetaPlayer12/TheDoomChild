using System;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{

    /// <summary>
    /// This looks more of a hacks than anything
    /// </summary>
    public class ImpactFXSpawner : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private BaseColliderDamage m_colliderDamage;
        [SerializeField]
        private FXSpawner m_fx;
        [SerializeField]
        private PolygonCollider2D[] m_colliders;

        private Character m_character;
        private FX m_cacheFX;
        private Vector3 m_fxScale;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_colliderDamage.DamageableDetected += OnDamageableDetected;
            m_fxScale = m_fx.fx.transform.localScale;
        }

        private void OnDamageableDetected(TargetInfo targetInfo,Collider2D obj)
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                if (m_colliders[i].enabled)
                {
                    var collider = m_colliders[i];
                    m_fx.spawnPoint.position = obj.ClosestPoint(collider.bounds.center);
                    m_cacheFX = m_fx.SpawnFX();
                    var scale = m_fxScale;
                    scale.x *= m_character.facing == HorizontalDirection.Left ? -1 : 1;
                    m_cacheFX.transform.localScale = scale;
                }
            }
        }
    }
}