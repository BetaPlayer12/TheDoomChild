using UnityEngine;

namespace DChild.Gameplay
{
    public class SpawnMultiArea : SpawnArea
    {
        [SerializeField]
        private SpawnArea[] m_areas;

        public override Vector2 GetRandomPosition()
        {
            var areaIndex = Random.Range(0, m_areas.Length);
            return m_areas[areaIndex].GetRandomPosition();
        }
    }
}