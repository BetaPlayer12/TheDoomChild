using DChild.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Testing.PreAlpha
{
    [AddComponentMenu(PreAlphaUtility.COMPONENTMENU_ADDRESS + "PlayTimeTracker")]
    public class PlayTimeTracker : SerializedMonoBehaviour
    {
        public struct SaveData
        {
            [SerializeField]
            private int m_hour;
            [SerializeField]
            private int m_minute;
            [SerializeField]
            private float m_seconds;

            public SaveData(int hour, int minute, float seconds)
            {
                m_hour = hour;
                m_minute = minute;
                m_seconds = seconds;
            }

            public int hour => m_hour;
            public int minute => m_minute;
            public float seconds => m_seconds;
        }

        [SerializeField]
        private int m_hour;
        [SerializeField]
        private int m_minute;
        [SerializeField]
        private float m_seconds;

        public SaveData Save() => new SaveData(m_hour, m_minute, m_seconds);

        public void Load(SaveData data)
        {
            m_hour = data.hour;
            m_minute = data.minute;
            m_seconds = data.seconds;
        }
        public void Update()
        {
            if (GameplaySystem.isGamePaused == false)
            {
                m_seconds += Time.deltaTime;
                if(m_seconds >= 60f)
                {
                    m_minute += 1;
                    m_seconds -= 60f;
                    if (m_minute >= 60f)
                    {
                        m_hour += 1;
                        m_minute -= 60;
                    }
                }
            }
        }
    }

}