using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Configurations
{
    public class ColoredCollider : MonoBehaviour
    {
#if UNITY_EDITOR
        [Flags]
        private enum ColliderType
        {
            Box = 1 << 0,
            Circle = 1 << 1,
            Edge = 1 << 2,
            Polygon = 1 << 3,
        }

        [SerializeField, HideLabel]
        public ColliderColor gizmoColor;
        [SerializeField]
        private bool m_considerChildrenCollider;
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

            if (m_considerChildrenCollider)
            {
                var colliders = GetComponentsInChildren<Collider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    DrawGizmoFor(colliders[i]);
                }
            }
            else
            {
                DrawGizmoFor(GetComponent<Collider2D>());
            }

            Gizmos.color = oldGizmosColor;
            Gizmos.matrix = oldGizmosMatrix;
        }

        private void DrawGizmoFor(Collider2D collider)
        {
            if (m_colliderType.HasFlag(ColliderType.Box) && collider is BoxCollider2D)
            {
                var boxCol = (BoxCollider2D)collider;
                if (boxCol)
                {
                    Gizmos.DrawWireCube(boxCol.offset, boxCol.size);
                }
            }

            if (m_colliderType.HasFlag(ColliderType.Circle) && collider is CircleCollider2D)
            {
                var circleCol = (CircleCollider2D)collider; ;
                if (circleCol)
                {
                    Gizmos.DrawWireSphere(circleCol.offset, circleCol.radius);
                }
            }


            if (m_colliderType.HasFlag(ColliderType.Polygon) && collider is PolygonCollider2D)
            {
                var polygonCol = (PolygonCollider2D)collider;
                if (polygonCol)
                {
                    DrawLines(polygonCol.points);
                }
            }

            if (m_colliderType.HasFlag(ColliderType.Edge) && collider is EdgeCollider2D)
            {
                var edgeCol = (EdgeCollider2D)collider;
                if (edgeCol)
                {
                    DrawLines(edgeCol.points);
                }
            }
        }

        private void DrawLines(Vector2[] points)
        {
            for (int i = 1; i < points.Length; i++)
            {
                Gizmos.DrawLine(points[i - 1], points[i]);
            }
            Gizmos.DrawLine(points[points.Length - 1], points[0]);
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