using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.UI
{
    [System.Serializable]
    public struct IndexSliderInterpreter
    {
        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = true)]
        private float[] m_output;

        public float GetOutput(int index)
        {
            return m_output[index];
        }

        public float InterpretOutput(float output)
        {
            float closestInterpretationIndex = 0;
            float closestInterpretationDistance = Mathf.Abs(m_output[0] - output);

            for (int i = 1; i < m_output.Length; i++)
            {
                var interpretationDistance = Mathf.Abs(m_output[i] - output);
                if (interpretationDistance == 0)
                {
                    closestInterpretationIndex = i;
                    break;
                }
                else if (interpretationDistance < closestInterpretationDistance)
                {
                    closestInterpretationDistance = interpretationDistance;
                    closestInterpretationIndex = i;
                }
            }

            return closestInterpretationIndex;
        }
    }

}