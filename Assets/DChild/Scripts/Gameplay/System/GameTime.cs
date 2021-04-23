using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public static class GameTime
    {
        public enum Factor
        {
            Addition,
            Multiplication
        }

        private static List<object> m_multipliedFactorsReference = new List<object>();
        private static List<float> m_multipliedFactors = new List<float>();
                 
        private static List<object> m_addensReference = new List<object>();
        private static List<float> m_addAddens = new List<float>();
                 
        private static float m_totalMultipliedFactors = 1;
        private static float m_totalSum = 0;

        public static void RegisterValueChange(object reference, float value, Factor factor)
        {
            if (factor == Factor.Multiplication)
            {
                if (m_multipliedFactorsReference.Contains(reference))
                {
                    var index = m_multipliedFactorsReference.FindIndex(x => (x == reference));
                    m_multipliedFactors[index] = value;
                }
                else
                {
                    m_multipliedFactorsReference.Add(reference);
                    m_multipliedFactors.Add(value);
                }
                TotalMultipliedFactors();
            }
            else
            {
                if (m_addensReference.Contains(reference))
                {
                    var index = m_addensReference.FindIndex(x => (x == reference));
                    m_addAddens[index] = value;
                }
                else
                {
                    m_addensReference.Add(reference);
                    m_addAddens.Add(value);
                }
                CalculateTotalSum();
            }

            Time.timeScale = m_totalMultipliedFactors + m_totalSum;
        }

        public static void UnregisterValueChange(object reference, Factor factor)
        {
            if (factor == Factor.Multiplication)
            {
                if (m_multipliedFactorsReference.Contains(reference))
                {
                    var index = m_multipliedFactorsReference.FindIndex(x => (x == reference));
                    m_multipliedFactorsReference.RemoveAt(index);
                    m_multipliedFactors.RemoveAt(index);
                    TotalMultipliedFactors();
                }
            }
            else
            {
                if (m_addensReference.Contains(reference))
                {
                    var index = m_addensReference.FindIndex(x => (x == reference));
                    m_addensReference.RemoveAt(index);
                    m_addAddens.RemoveAt(index);
                    TotalMultipliedFactors();
                }
                CalculateTotalSum();
            }
            Time.timeScale = m_totalMultipliedFactors + m_totalSum;
        }

        private static void CalculateTotalSum()
        {
            m_totalSum = 0;
            for (int i = 0; i < m_addAddens.Count; i++)
            {
                m_totalSum += m_addAddens[i];
            }
        }

        private static void TotalMultipliedFactors()
        {
            m_totalMultipliedFactors = 1;
            for (int i = 0; i < m_multipliedFactors.Count; i++)
            {
                m_totalMultipliedFactors *= m_multipliedFactors[i];
            }
        }
    }

}