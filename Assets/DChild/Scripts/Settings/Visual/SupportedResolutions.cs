using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Configurations.Visuals
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SupportedResolutions", menuName = "DChild/System/Supported Resolutions")]
    public class SupportedResolutions : ScriptableObject
    {
        [SerializeField]
        private Resolution[] m_resolutions;

        public Resolution[] GetResolutions()
        {
            Resolution[] toReturn = new Resolution[m_resolutions.Length];
            Array.Copy(m_resolutions, toReturn, m_resolutions.Length);
            return toReturn;
        }
        public Resolution GetResolution(int index) => m_resolutions[index];
    }
}
