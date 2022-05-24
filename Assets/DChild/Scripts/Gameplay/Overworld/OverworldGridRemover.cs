using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Overworld
{
    public class OverworldGridRemover : MonoBehaviour
    {
        [SerializeField]
        private float 
        public event Action<Vector2Int> GridIndexRemoved;

        private Dictionary<GameObject, Vector2Int> m_forRemovalPair;
        private List<Transform> m_grids;

        public void ProcessRemoval(Vector2Int index, GameObject tile)
        {
            if(m_forRemovalPair.ContainsKey(tile) == false)
            {
                m_forRemovalPair.Add(tile, index);
            }

            Destroy(tile);
            GridIndexRemoved?.Invoke(index);
        }

        public void RecindRemoval(Vector2Int index, GameObject tile)
        {

        }

        private void Start()
        {
            
        }

        private void LateUpdate()
        {
            
        }
    }

}