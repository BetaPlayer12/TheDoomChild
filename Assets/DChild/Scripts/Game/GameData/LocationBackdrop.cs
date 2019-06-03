using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DChild
{
    [CreateAssetMenu(fileName = "LocationBackdrop", menuName = "DChild/System/Location Backdrop")]
    public class LocationBackdrop : ScriptableObject, IGameData
    {
        [System.Serializable]
        public class Data
        {
#if UNITY_EDITOR
            [SerializeField]
            [ReadOnly]
            private Location m_name;

            public Data(Location m_name)
            {
                this.m_name = m_name;
            }

            public Location name { set { m_name = value; } }
#endif
            [SerializeField]
            private Sprite m_backdrop;

            public Sprite backdrop { get => m_backdrop; }
        }


        [SerializeField]
        [PropertyOrder(1)]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        [OnValueChanged("OnDataChanged")]
        private Data[] m_data;

        public Sprite GetBackdrop(Location location) => m_data[(int)location].backdrop;



#if UNITY_EDITOR
        [Button("Initialize")]
        [PropertyOrder(0)]
        private void Initialize()
        {
            m_data = new Data[(int)Location._COUNT];
            for (int i = 0; i < (int)Location._COUNT; i++)
            {
                m_data[i] = new Data((Location)i);
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private bool OnDataChanged()
        {
            for (int i = 0; i < m_data.Length; i++)
            {
                m_data[i].name = ((Location)i);
            }
            return true;
        }
#endif
    }
}