using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class FPSLog
    {
        private List<float> m_fps;

        public float minFPS => Mathf.Min(m_fps.ToArray());
        public float maxFPS => Mathf.Max(m_fps.ToArray());
        public float aveFPS
        {
            get
            {
                var total = 0f;
                for (int i = 0; i < m_fps.Count; i++)
                {
                    total += m_fps[i];
                }

                return total / m_fps.Count;
            }
        }

        public FPSLog()
        {
            m_fps = new List<float>();
        }

        public void ClearLog()
        {
            m_fps.Clear();
        }

        public void Log(float fps)
        {
            m_fps.Add(fps);
        }
    }
}