using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    public class WorldPhysicsObjects : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private PhysicsObjects m_physicsObjects;

        private void OnEnable()
        {
            if (m_physicsObjects != null)
            {
                GameplaySystem.world.Register(m_physicsObjects);
            }
        }

        private void OnDisable()
        {
            if (m_physicsObjects != null)
            {
                GameplaySystem.world.Unregister(m_physicsObjects);
            }
        }

        private void OnValidate()
        {
            var rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            if (rigidbodies == null)
            {
                m_physicsObjects = null;
            }
            else
            {
                m_physicsObjects = new PhysicsObjects(rigidbodies);
            }
        }
    }
}