using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{

    public class ShadowEnvironmentHandle : MonoBehaviour
    {
        [SerializeField,InfoBox("These colliders will be ignored by the player when he is in SHADOW mode")]
        private Collider2D[] m_shadowColliders;
        private Collider2D[] m_playerColliders;

        public void SetCollisions(bool enableCollisions)
        {
            for (int i = 0; i < m_playerColliders.Length; i++)
            {
                for (int j = 0; j < m_shadowColliders.Length; j++)
                {
                    Physics2D.IgnoreCollision(m_playerColliders[i], m_shadowColliders[j], enableCollisions);
                }
            }
        }

        private void Start()
        {
            m_playerColliders = GameplaySystem.playerManager.player.character.colliders.colliders;
            GameplaySystem.world.Register(this);
        }
    }
}