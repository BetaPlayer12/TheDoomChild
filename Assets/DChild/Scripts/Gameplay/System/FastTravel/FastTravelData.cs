using DChild.Gameplay.Systems.Serialization;
using Holysoft.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.FastTravel
{
    [CreateAssetMenu(fileName = "FastTravelData", menuName = "DChild/Gameplay/Fast Travel/FastTravel Data")]
    public class FastTravelData : ScriptableObject
    {
        [SerializeField]
        private LocationData m_fastTravelPoint;
        [SerializeField]
        private string m_pointName;
        [SerializeField]
        private Sprite m_image;

        public LocationData fastTravelPoint => m_fastTravelPoint;
        public string pointName => m_pointName;
        public Sprite image => image;
    } 
}
