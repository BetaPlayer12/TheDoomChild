using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Overworld
{
    public class OverworldGridRemover : MonoBehaviour
    {
        [SerializeField]
        private Transform m_reference;
        [SerializeField]
        private float m_distanceFromGridThreshold;
        public event Action<Vector2Int> GridIndexRemoved;

        private Dictionary<GameObject, Vector2Int> m_forRemovalPair;
        private List<Transform> m_grids;

        public void ProcessRemoval(Vector2Int index, GameObject tile)
        {
            if (m_forRemovalPair.ContainsKey(tile) == false)
            {
                m_forRemovalPair.Add(tile, index);
                m_grids.Add(tile.transform);
            }
        }

        public void RecindRemoval(Vector2Int index, GameObject tile)
        {
            if (m_forRemovalPair.ContainsKey(tile))
            {
                m_forRemovalPair.Remove(tile);
                m_grids.Remove(tile.transform);
            }
        }

        private void Start()
        {
            m_forRemovalPair = new Dictionary<GameObject, Vector2Int>();
            m_grids = new List<Transform>();
        }

        private void LateUpdate()
        {
            for (int i = m_grids.Count - 1; i >= 0; i--)
            {
                var currentGrid = m_grids[i];
                if (Vector3.Distance(currentGrid.position, m_reference.position) >= m_distanceFromGridThreshold)
                {
                    var tile = currentGrid.gameObject;
                    Destroy(tile);
                    GridIndexRemoved?.Invoke(m_forRemovalPair[tile]);
                }
            }
        }
    }

}