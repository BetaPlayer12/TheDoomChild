using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerSensorList : SerializedMonoBehaviour
    {
        public enum SensorType
        {
            Head,
            Ground,
            Wall,
            Slope,
            WallStick,
            GroundHeight,
            LedgeCliff,
            LedgeEdge,
            Platform
        }

        [SerializeField]
        private Dictionary<SensorType, RaySensor> m_list;

        public RaySensor GetSensor(SensorType sensorType) => m_list[sensorType];
    }
}