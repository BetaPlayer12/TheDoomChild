using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [CreateAssetMenu(fileName = "ThrowableItemData", menuName = "DChild/Database/Throwable Item Data")]
    public class ThrowableItemData : ConsumableItemData
    {
        [SerializeField, ToggleGroup("m_enableEdit")]
        private ProjectileInfo m_projectile;
        private static bool m_isOnCooldown;

        public override bool CanBeUse(IPlayer player)
        {
            return m_isOnCooldown == false && player.state.isAttacking == false;
        }

        public override void Use(IPlayer player)
        {
            var handle = player.character.GetComponentInChildren<ProjectileThrow>();
            handle.SetProjectileInfo(m_projectile);
            handle.RequestExecution();
        }
    }
}
