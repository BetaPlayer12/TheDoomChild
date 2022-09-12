using UnityEngine;

namespace DChild.Inputs
{
    [System.Serializable]
    public struct MouseInput : IInput
    {
        private Vector2 m_shiftNormal;
        private Vector2 m_shiftNormalRaw;
        private Vector2 m_position;

        public Vector2 shiftNormal => m_shiftNormal;
        public Vector2 shiftNormalRaw => m_shiftNormalRaw;

        public void Initialize()
        {
            m_position = Input.mousePosition;
        }

        public void Disable()
        {
            m_shiftNormal = Vector2.zero;
            m_shiftNormalRaw = Vector2.zero;
            m_position = Vector2.zero;
        }

        public void Update()
        {
            Vector2 currentPosition = Input.mousePosition;

            m_shiftNormal = (currentPosition - m_position).normalized;
            m_shiftNormalRaw.x = GetRawValue(m_shiftNormal.x);
            m_shiftNormalRaw.y = GetRawValue(m_shiftNormal.y);

            m_position = currentPosition;
        }

        private int GetRawValue(float value)
        {
            if (value == 0)
            {
                return 0;
            }
            else if (value > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }

}