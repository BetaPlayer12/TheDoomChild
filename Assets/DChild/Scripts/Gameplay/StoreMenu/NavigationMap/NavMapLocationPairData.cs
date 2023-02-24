using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    [CreateAssetMenu(fileName = "NavMapLocationPairData", menuName = "DChild/Gameplay/Combat/NavMapLocationPair Data")]
    public class NavMapLocationPairData : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<Location, GameObject> m_locationMapPair;

        public GameObject GetValue(Location currentLocation)
        {
            return m_locationMapPair[currentLocation].gameObject;
        }
    }
}

