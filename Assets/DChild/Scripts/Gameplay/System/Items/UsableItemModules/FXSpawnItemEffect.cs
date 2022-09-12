using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public class FXSpawnItemEffect : IUsableItemModule
    {
        [SerializeField]
        private GameObject m_fx;
        [SerializeField]
        private BodyReference.BodyPart m_spawnAtBodyPart;

        private static FXSpawnHandle<FX> fxSpawner = new FXSpawnHandle<FX>();

        public bool CanBeUse(IPlayer player) => true;

        public void Use(IPlayer player)
        {
            var character = player.character;
            var bodyPart = character.GetBodyPart(m_spawnAtBodyPart);
            var fx = fxSpawner.InstantiateFX(m_fx, bodyPart.position, character.facing);
            fx.transform.SetParent(bodyPart);
        }
    }
}
