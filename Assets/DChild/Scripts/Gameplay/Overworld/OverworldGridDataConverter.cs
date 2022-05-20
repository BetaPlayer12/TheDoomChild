using Sirenix.OdinInspector;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DChild.Gameplay.Overworld
{
    public class OverworldGridDataConverter : MonoBehaviour
    {
        [SerializeField]
        private OverworldGridData m_data;
        [SerializeField]
        private Vector2 m_gridsize;
        [SerializeField]
        private Transform m_scope;

#if UNITY_EDITOR
        [Button]
        private void Convert()
        {
            var objectCount = m_scope.childCount;
            GameObject[] grids = new GameObject[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                grids[i] = m_scope.GetChild(i).gameObject;
            }

            var gridsXPositions = grids.Select(x => x.transform.position.x).ToArray();
            var minXValue = Mathf.Min(gridsXPositions);
            var maxXValue = Mathf.Max(gridsXPositions);
            var gridXCount = GetGridDimensionCount(minXValue, maxXValue) + 1;

            var gridsYPositions = grids.Select(x => x.transform.position.y).ToArray();
            var minYValue = Mathf.Min(gridsYPositions);
            var maxYValue = Mathf.Max(gridsYPositions);
            var gridYCount = GetGridDimensionCount(minYValue, maxYValue) + 1;

            OverworldGridCalculator calculator = new OverworldGridCalculator(m_gridsize, minXValue, maxYValue);

            var gridData = new GameObject[gridXCount, gridYCount];
            for (int i = 0; i < grids.Length; i++)
            {
                var grid = grids[i];
                var gridPosition = grid.transform.position;
                var index = calculator.GetGridIndex(gridPosition);
                gridData[index.x, index.y] = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(grid);
            }

            m_data.SetData(gridData);
        }

        private int GetGridDimensionCount(float minDimensionValue, float maxDimensionValue)
        {
            return GetIndex(Mathf.Abs(minDimensionValue), m_gridsize.x) + GetIndex(Mathf.Abs(maxDimensionValue), m_gridsize.x);
        }

        private int GetIndex(float value, float modifier)
        {
            if (value == 0)
            {
                return 0;
            }
            else
            {
                return Mathf.FloorToInt(value / modifier);
            }
        }
#endif

    }

}