using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public abstract class Loot : PoolableObject
    {
        public static string objectTag => "Loot";

        public abstract void PickUp(IPlayer player);

        private void OnValidate()
        {
            if (gameObject.CompareTag(objectTag) == false)
            {
                gameObject.tag = objectTag;
                Debug.Log(gameObject.tag);
            }
        }
    }
}