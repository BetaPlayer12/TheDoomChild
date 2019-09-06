using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public abstract class Loot : PoolableObject
    {
        [SerializeField]
        private Rigidbody2D m_rigidbody;

        public static string objectTag => "Loot";

        public abstract void PickUp(IPlayer player);
        public void Pop(Vector2 force)
        {
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
        private void OnValidate()
        {
            //if (gameObject.CompareTag(objectTag) == false)
            //{
            //    gameObject.tag = objectTag;
            //    Debug.Log(gameObject.tag);
            //}
        }
    }
}