using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Overworld
{

    public class OverworldGridData : SerializedMonoBehaviour
    {
        [SerializeField,TableMatrix(SquareCells = true)]
        private GameObject[,] m_gridTiles;

        public void SetData(GameObject[,] data)
        {
            m_gridTiles = data;
        }
    }

}