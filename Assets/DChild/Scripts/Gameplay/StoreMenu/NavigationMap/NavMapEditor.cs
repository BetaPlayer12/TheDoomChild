using DChild.Gameplay.NavigationMap;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapEditor : MonoBehaviour
    {

        [SerializeField]
        private NavMapFOWSerializationConfigurator m_fowSerializationConfig;
        [SerializeField]
        private NavMapPOISerializationConfigurator m_poiSerializationConfig;

        [SerializeField, ReadOnly]
        private Environment.Location m_fowLocation;

        [SerializeField, ReadOnly]
        private Environment.Location m_poiLocation;

        [SerializeField]
        private bool m_includePOI;

        [SerializeField]
        private bool m_includeFOW;


            [Button]
        private void SerializeToDatabase()
        {
#if UNITY_EDITOR
            if (m_includePOI)
            {

                m_poiSerializationConfig.SerializeToDatabase();
            }
            if (m_includeFOW)
            {
  
                m_fowSerializationConfig.SerializeToDatabase();
            }
            else
            {

            }

#endif
        }


        private void Start()
        {
            m_poiLocation = m_poiSerializationConfig.location;
            m_fowLocation = m_fowSerializationConfig.location;
        }

    }
}

