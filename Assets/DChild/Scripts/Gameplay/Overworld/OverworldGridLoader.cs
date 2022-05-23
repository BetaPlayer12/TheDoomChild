using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Overworld
{
    public class OverworldGridLoader : MonoBehaviour
    {
        [SerializeField]
        private OverworldGridData m_gridData;
        [SerializeField, BoxGroup("Load Simulator")]
        private Transform m_referencePoint;
        [SerializeField, BoxGroup("Load Simulator")]
        private Vector2Int m_visibleGrids;

        private Vector2Int m_currentIndex = new Vector2Int(-1, -1);
        private Dictionary<Vector2Int, GameObject> m_activeTilesPair;

        [SerializeField]
        private List<Vector2Int> m_activeTiles;
        private List<Vector2Int> m_newActiveTiles;

        private void RecordVisibleGrids(Vector2Int currentIndex)
        {
            var gridSize = m_gridData.GetGridSize();
            for (int x = -m_visibleGrids.x; x <= m_visibleGrids.x; x++)
            {
                var tileIndex = currentIndex;
                var initialY = tileIndex.y;
                tileIndex.x += x;

                if (tileIndex.x < 0 || tileIndex.x >= gridSize.x)
                    continue;

                for (int y = -m_visibleGrids.y; y <= m_visibleGrids.y; y++)
                {
                    tileIndex.y = initialY;
                    tileIndex.y += y;

                    if (tileIndex.y < 0 || tileIndex.y >= gridSize.y)
                        continue;

                    if (x == 0 && y == 0)
                        continue;


                    m_newActiveTiles.Add(tileIndex);
                }
            }
        }

        private void AddNewVisibleTiles()
        {
            for (int i = 0; i < m_newActiveTiles.Count; i++)
            {
                var currentTileIndex = m_newActiveTiles[i];
                if (m_activeTilesPair.ContainsKey(currentTileIndex) == false)
                {
                    var prefab = m_gridData.GetTile(currentTileIndex);
                    if (prefab == null)
                        continue;

                    var tile = Instantiate(prefab, transform);
                    m_activeTilesPair.Add(currentTileIndex, tile);
                }
            }
        }

        private void Start()
        {
            m_activeTilesPair = new Dictionary<Vector2Int, GameObject>();
            m_activeTiles = new List<Vector2Int>();
            m_newActiveTiles = new List<Vector2Int>();
        }

        private void LateUpdate()
        {
            var currentIndex = m_gridData.GetIndex(m_referencePoint.position);
            if (currentIndex != m_currentIndex)
            {
                m_newActiveTiles.Add(currentIndex);
                m_currentIndex = currentIndex;

                RecordVisibleGrids(currentIndex);

                GameObject cacheTile;
                for (int i = 0; i < m_activeTiles.Count; i++)
                {
                    var currentTileIndex = m_activeTiles[i];
                    if (m_newActiveTiles.Contains(currentTileIndex) == false)
                    {
                        if (m_activeTilesPair.TryGetValue(currentTileIndex,out cacheTile))
                        {
                            Destroy(cacheTile);
                            m_activeTilesPair.Remove(currentTileIndex);
                        }
                    }
                }

                AddNewVisibleTiles();

                m_activeTiles.Clear();
                m_activeTiles.AddRange(m_newActiveTiles);
                m_newActiveTiles.Clear();
            }
        }


    }

}