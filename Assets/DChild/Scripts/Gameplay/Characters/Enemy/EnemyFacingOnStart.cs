using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyFacingOnStart : MonoBehaviour
    {
        [SerializeField]
        private bool m_faceEnemyOnStart;
        private Enemy m_enemy;

        private void OnEnable()
        {
            m_enemy = GetComponent<Enemy>();

            if (m_faceEnemyOnStart)
            {
                Vector2 target = Vector2.zero/*= GameplaySystem.playerManager.player.position*/;
                var position = m_enemy.position;
                var facing = m_enemy.currentFacingDirection;

                if (position.x > target.x && facing != HorizontalDirection.Left)
                {
                    m_enemy.SetFacing(HorizontalDirection.Left);
                }
                else if (position.x < target.x && facing != HorizontalDirection.Right)
                {
                    m_enemy.SetFacing(HorizontalDirection.Right);
                }
            }
            enabled = false;
        }
    }
}
