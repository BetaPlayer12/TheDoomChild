using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class SpawnArea : MonoBehaviour
    {
        public abstract Vector2 GetRandomPosition();
        protected float RandomNormal() => UnityEngine.Random.Range(-1f, 1f);

        private void OnValidate()
        {
            gameObject.name = GetType().Name;
        }
    }
}