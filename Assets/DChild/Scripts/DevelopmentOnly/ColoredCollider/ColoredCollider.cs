using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Configurations
{
    public class ColoredCollider : MonoBehaviour
    {
        [Flags]
        private enum ColliderType
        {
            Box,
            Circle,
            Edge,
            Polygon
        }

#if UNITY_EDITOR
        [SerializeField, HideLabel]
        public ColliderColor gizmoColor;
        [SerializeField]
        private ColliderType m_colliderType;
        private static bool instancesMentioned;

        private void Start()
        {
            if (instancesMentioned == false)
            {
                Debug.LogError("Colored Collider Components are Present");
                instancesMentioned = true;
            }
        }

        private void DrawGizmos()
        {
            var oldGizmosColor = Gizmos.color;
            Gizmos.color = gizmoColor.color;
            var oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            if (m_colliderType.HasFlag(ColliderType.Box))
            {
                var boxCol = GetComponent<BoxCollider2D>();
                if (boxCol)
                {
                    Gizmos.DrawWireCube(boxCol.offset, boxCol.size);
                }
            }

            if (m_colliderType.HasFlag(ColliderType.Circle))
            {
                var circleCol = GetComponent<CircleCollider2D>();
                if (circleCol)
                {
                    Gizmos.DrawWireSphere(circleCol.offset, circleCol.radius);
                }
            }


            if (m_colliderType.HasFlag(ColliderType.Polygon))
            {
                var polygonCol = GetComponent<PolygonCollider2D>();
                if (polygonCol)
                {
                    DrawLines(polygonCol.points);
                }
            }

            if (m_colliderType.HasFlag(ColliderType.Edge))
            {
                var edgeCol = GetComponent<EdgeCollider2D>();
                if (edgeCol)
                {
                    DrawLines(edgeCol.points);
                }
            }

            Gizmos.color = oldGizmosColor;
            Gizmos.matrix = oldGizmosMatrix;
        }

        private void DrawLines(Vector2[] points)
        {
            for (int i = 1; i < points.Length; i++)
            {
                Gizmos.DrawLine(points[i - 1], points[i]);
            }
        }

        private void OnDrawGizmos()
        {
            if (Physics2D.alwaysShowColliders)
            {
                DrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Physics2D.alwaysShowColliders == false)
            {
                DrawGizmos();
            }
        }
#endif

        private void OnValidate()
        {
            if (Application.isPlaying && Application.isEditor == false)
            {
                Destroy(this);
            }
        }
    }
}