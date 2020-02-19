using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems
{
    public struct LootDropRequest
    {
        public GameObject loot { get; }
        public int count;
        public Vector2 location { get; }
        public Scene sceneToDropLoot { get; }

        public LootDropRequest(GameObject loot, int count, Vector2 location, Scene sceneToDropLoot)
        {
            this.loot = loot;
            this.count = count;
            this.location = location;
            this.sceneToDropLoot = sceneToDropLoot;
        }
    }
}