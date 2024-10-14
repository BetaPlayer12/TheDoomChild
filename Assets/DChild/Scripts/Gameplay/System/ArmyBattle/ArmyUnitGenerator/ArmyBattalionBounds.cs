using UnityEditor;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Battalion
{
    [System.Serializable]
    public struct ArmyBattalionBounds
    {
        public Vector2 size;
        [HideInInspector]
        public Vector2 center;

        [SerializeField]
        private Color m_debugColor;

        public Vector2 extent => size / 2;
        public Vector2 min => center - extent;
        public Vector2 max => center + extent;

        public void DrawGizmos()
        {
            Gizmos.color = m_debugColor;
            Gizmos.DrawCube(center, size);
        }
    }
}