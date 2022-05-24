using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Overworld
{

    [CreateAssetMenu(fileName = "OverworldGridData", menuName = "DChild/Overworld Grid Data")]
    public class OverworldGridData : SerializedScriptableObject
    {
        [SerializeField]
        private OverworldGridCalculator m_calculator;
        [SerializeField, TableMatrix(SquareCells = true)]
        private GameObject[,] m_gridTiles;

        public void SetData(OverworldGridCalculator calculator, GameObject[,] data)
        {
            m_calculator = calculator;
            m_gridTiles = data;
        }

        public Vector2Int GetGridSize() => new Vector2Int(m_gridTiles.GetLength(0), m_gridTiles.GetLength(1));

        public GameObject GetTile(Vector2Int index)
        {
            if (index.x < 0 || index.y < 0 || index.x >= m_gridTiles.GetLength(0) || index.y >= m_gridTiles.GetLength(1))
                return null;

            return m_gridTiles[index.x, index.y];
        }

        public Vector2Int GetIndex(Vector2 position) => m_calculator.GetGridIndex(position);
    }

}