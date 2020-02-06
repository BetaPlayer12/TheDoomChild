using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class SpawnArea : MonoBehaviour
    {
        public abstract Vector2 GetRandomPosition();
        protected float RandomNormal() => Random.Range(-1f, 1f);

        private void OnValidate()
        {
            gameObject.name = GetType().Name;
        }
    }
}