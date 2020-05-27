using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [CreateAssetMenu(fileName = "ThrowableItemData", menuName = "DChild/Database/Throwable Item Data")]
    public class ThrowableItemData : ConsumableItemData
    {
        [SerializeField]
        private GameObject m_projectile;

        public override bool CanBeUse(IPlayer player)
        {
            return true;
        }

        public override void Use(IPlayer player)
        {
            var handle = player.character.GetComponent<ProjectileThrowHandler>();
            handle.SetProjectile(m_projectile);
            handle.Initialize();
            /*Start Throw via Controller
             Controller decides when to spawn the projectile
             this only starts the process*/
        }
    }
}
