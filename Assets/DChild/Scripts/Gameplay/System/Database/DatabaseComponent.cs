using UnityEngine;

namespace DChild.Gameplay.Databases
{
    public abstract class DatabaseComponent<T> : MonoBehaviour, IDatabaseComponent where T : IDatabase
    {
        [SerializeField]
        private T m_database;

        public IDatabase database => m_database;

#if UNITY_EDITOR
        public void Initialize(T database)
        {
            m_database = database;
        }
#endif
    }
}