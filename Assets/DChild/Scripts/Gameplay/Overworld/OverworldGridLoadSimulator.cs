using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Overworld
{

    public class OverworldGridLoadSimulator : SerializedMonoBehaviour

    {
        [SerializeField, BoxGroup("Load Simulator")]
        private Transform m_referencePoint;
        [SerializeField, BoxGroup("Load Simulator")]
        private Vector2Int m_visibleGrids;

        [SerializeField, BoxGroup("Setup")]
        private OverworldGridDataConverter m_converter;
        [SerializeField, ReadOnly, BoxGroup("Setup")]
        private OverworldGridCalculator m_calculator;
        [SerializeField, TableMatrix(SquareCells = true), ReadOnly, BoxGroup("Setup")]
        private GameObject[,] m_gridTiles;

        private Vector2Int m_currentIndex = new Vector2Int(-1, -1);
        private HashSet<GameObject> activeTiles;
        private HashSet<GameObject> newActiveTiles;

        private GameObject GetTile(Vector2Int index)
        {
            if (index.x < 0 || index.y < 0 || index.x >= m_gridTiles.GetLength(0) || index.y >= m_gridTiles.GetLength(1))
                return null;

            return m_gridTiles[index.x, index.y];
        }

        public void Start()
        {
            var data = m_converter.GetData();
            m_calculator = data.calculator;

            for (int i = 0; i < data.grids.Length; i++)
            {
                var grid = data.grids[i];
                var gridPosition = grid.transform.position;
                var index = data.calculator.GetGridIndex(gridPosition);
                data.gridData[index.x, index.y] = grid;
                grid.SetActive(false);
            }
            m_gridTiles = data.gridData;
            activeTiles = new HashSet<GameObject>();
            newActiveTiles = new HashSet<GameObject>();
        }

        private void LateUpdate()
        {
            var currentIndex = m_calculator.GetGridIndex(m_referencePoint.position);
            if (currentIndex != m_currentIndex)
            {
                for (int i = 0; i < activeTiles.Count; i++)
                {
                    activeTiles.ElementAt(i).SetActive(false);
                }
                activeTiles.Clear();

                ActivateTile(currentIndex);
                m_currentIndex = currentIndex;

                for (int x = -m_visibleGrids.x; x <= m_visibleGrids.x; x++)
                {
                    for (int y = -m_visibleGrids.y; y <= m_visibleGrids.y; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        var tileIndex = currentIndex;
                        tileIndex.x += x;
                        tileIndex.y += y;
                        ActivateTile(tileIndex);
                    }
                }

            }
        }

        private void ActivateTile(Vector2Int currentIndex)
        {
            var tile = GetTile(currentIndex);
            if (tile != null)
            {
                tile.SetActive(true);
                activeTiles.Add(tile);
            }
        }
    }

}