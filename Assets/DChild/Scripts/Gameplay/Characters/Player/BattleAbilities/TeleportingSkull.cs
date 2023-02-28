using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class TeleportingSkull : AttackBehaviour
    {
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectile;
        public ProjectileInfo projectile => m_projectile;
        private Projectile m_spawnedProjectile;
        private bool m_canTeleport;
        public bool canTeleport => m_canTeleport;

        public void GetSpawnedProjectile(Projectile spawnedProjectile)
        {
            if (spawnedProjectile != null)
            {
                m_spawnedProjectile = spawnedProjectile;
            }
        }

        public void TeleportToProjectile()
        {
            m_canTeleport = false;
            m_character.transform.position = m_spawnedProjectile.transform.position;
            m_spawnedProjectile.CallPoolRequest();
        }

        public void Execute()
        {
            m_canTeleport = true;
        }
    }
}
