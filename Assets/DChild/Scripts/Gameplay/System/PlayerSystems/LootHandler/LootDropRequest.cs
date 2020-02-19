using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public struct LootDropRequest
    {
        public GameObject loot { get; }
        public int count;
        public Vector2 location { get; }

        public LootDropRequest(GameObject loot, int count, Vector2 location) : this()
        {
            this.loot = loot;
            this.count = count;
            this.location = location;
        }


    }
}