using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class WhipPassableHandle : MonoBehaviour
    {
        [SerializeField,SceneObjectsOnly]
        private Collider2D[] m_passableList;

        private void Start()
        {
            WhipColliderDamage.InitializePassables(m_passableList);
        }
    }
}