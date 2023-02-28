using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct TeleportSkull : ISoulSkillModule
    {
        [SerializeField]
        private ProjectileInfo m_teleportprojectile;
        [SerializeField]
        private ProjectileInfo m_projectile;
        [SerializeField]
        private ThrowableItemData m_teleportingprojectile;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            Character m_playercharacter = GameplaySystem.playerManager.player.character;
            ProjectileThrow projectilethrower = m_playercharacter.GetComponentInChildren<ProjectileThrow>();
            projectilethrower.SetProjectileInfo(m_teleportprojectile);
            var playerInventory = GameplaySystem.playerManager.player.inventory;
            playerInventory.AddItem(m_teleportingprojectile, 1);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            Character m_playercharacter = GameplaySystem.playerManager.player.character;
            ProjectileThrow projectilethrower = m_playercharacter.GetComponentInChildren<ProjectileThrow>();
            projectilethrower.SetProjectileInfo(m_projectile);
            var playerInventory = GameplaySystem.playerManager.player.inventory;
            playerInventory.RemoveItem(m_teleportingprojectile, 1);
        }
    }
}
