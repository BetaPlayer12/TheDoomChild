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
<<<<<<< HEAD
=======
            Platform
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
        }

        [SerializeField]
        private Dictionary<SensorType, RaySensor> m_list;

        public RaySensor GetSensor(SensorType sensorType) => m_list[sensorType];
    }
}