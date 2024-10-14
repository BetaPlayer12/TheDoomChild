using DChild.Gameplay.ArmyBattle.Battalion;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.ArmyBattle.Units
{
    [System.Serializable]
    public class ArmyUnitGeneratorConfiguration
    {
        [SerializeField, MinMaxSlider(0f, 1f, true), BoxGroup("Size")]
        private Vector2 m_sizeXPercent = Vector2.up;
        [SerializeField, MinMaxSlider(0f, 1f, true), BoxGroup("Size")]
        private Vector2 m_sizeYPercent = Vector2.up;
        [SerializeField, MinValue(1), BoxGroup("Grid")]
        private int m_gridSizeX;
        [SerializeField, MinValue(1), BoxGroup("Grid")]
        private int m_gridSizeY;

        [SerializeField]
        private Color m_debugColor = Color.white;
        [SerializeField]
        private GameObject[] m_unitTemplates;

        public GameObject GetRandomUnitTemplate() => m_unitTemplates[Random.Range(0, m_unitTemplates.Length)];

        private ArmyBattalionBounds GenerateRelativeBounds(ArmyBattalionBounds reference)
        {
            Vector2 minPoints = new Vector2(reference.size.x * m_sizeXPercent.x, reference.size.y * m_sizeYPercent.x);
            Vector2 maxPoints = new Vector2(reference.size.x * m_sizeXPercent.y, reference.size.y * m_sizeYPercent.y);

            var size = new Vector2(Mathf.Abs(minPoints.x - maxPoints.x), Mathf.Abs(minPoints.y - maxPoints.y));
            var center = new Vector2(minPoints.x + (size.x / 2), minPoints.y + (size.y / 2)) + reference.center - (reference.size / 2);

            return new ArmyBattalionBounds()
            {
                center = center,
                size = size
            };
        }

        public List<Vector2> GenerateGridPositions(ArmyBattalionBounds reference)
        {
            var gridPositions = new List<Vector2>();
            var relativeBounds = GenerateRelativeBounds(reference);

            var gridSpacing = Vector2.zero;
            gridSpacing.x = relativeBounds.size.x / (m_gridSizeX - 1);
            gridSpacing.y = relativeBounds.size.y / (m_gridSizeY - 1);

            for (int x = 0; x < m_gridSizeX; x++)
            {
                var xPosition = gridSpacing.x * x;
                if (m_gridSizeX == 1)
                {
                    xPosition = relativeBounds.extent.x;
                }

                for (int y = 0; y < m_gridSizeY; y++)
                {
                    var yPosition = gridSpacing.y * y;
                    if (m_gridSizeY == 1)
                    {
                        yPosition = relativeBounds.extent.y;
                    }

                    var positionOffset = new Vector2(xPosition, yPosition);
                    var position = relativeBounds.min + positionOffset;
                    gridPositions.Add(position);
                }
            }
            return gridPositions;
        }

        public void DrawGizmo(ArmyBattalionBounds reference)
        {
            Gizmos.color = m_debugColor;
            var relativeBounds = GenerateRelativeBounds(reference);
            Gizmos.DrawCube(relativeBounds.center, relativeBounds.size);

            var gridPositions = GenerateGridPositions(reference);
            for (int i = 0; i < gridPositions.Count; i++)
            {
                Gizmos.DrawSphere(gridPositions[i], 0.5f);
            }
        }
    }
}