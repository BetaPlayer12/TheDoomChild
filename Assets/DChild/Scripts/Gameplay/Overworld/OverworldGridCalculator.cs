using UnityEngine;

namespace DChild.Gameplay.Overworld
{
    [System.Serializable]
    public struct OverworldGridCalculator
    {
        [SerializeField]
        private Vector2 m_gridsize;
        [SerializeField]
        private float m_minXValue;
        [SerializeField]
        private float m_maxYValue;

        public OverworldGridCalculator(Vector2 gridsize, float minXValue, float maxYValue)
        {
            m_gridsize = gridsize;
            m_minXValue = minXValue;
            m_maxYValue = maxYValue;
        }

        public Vector2Int GetGridIndex(Vector2 position) => new Vector2Int(GetXIndex(position.x), GetYIndex(position.y));

        public int GetXIndex(float value) => GetIndex(value - m_minXValue, m_gridsize.x);

        public int GetYIndex(float value) => GetIndex(m_maxYValue - value, m_gridsize.y);

        public int GetIndex(float value, float modifier)
        {
            if (value == 0)
            {
                return 0;
            }
            else
            {
                var unrefinedIndex = value / modifier;
                var isWholeNumber = unrefinedIndex % 1 == 0;
                if (isWholeNumber)
                {
                    return (int)unrefinedIndex;
                }
                else
                {

                    //var floorIndex = Mathf.FloorToInt(unrefinedIndex);
                    //var ceilIndex = Mathf.CeilToInt(unrefinedIndex);
                    //var toFloorDistance = unrefinedIndex - unrefinedIndex;
                    //var toCeilDistance = unrefinedIndex - ceilIndex;

                    //if(toFloorDistance < toCeilDistance)

                    //return Mathf.FloorToInt(value / modifier);

                    return Mathf.RoundToInt(unrefinedIndex);
                }
            }
        }
    }

}