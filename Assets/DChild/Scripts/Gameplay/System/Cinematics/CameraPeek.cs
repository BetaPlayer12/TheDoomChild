using Sirenix.OdinInspector;
using UnityEngine;
using static DChild.Gameplay.Cinematics.Cinema;

namespace DChild.Gameplay.Cinematics
{
    public class CameraPeek : MonoBehaviour
    {
        [SerializeField, MinValue(0)]
        private float m_holdForPeek;
        private float m_holdTime;

        private bool m_isPeeking;

        private void LateUpdate()
        {
            var value = Input.GetAxisRaw("Vertical");
            switch (value)
            {
                case 0:
                    m_holdTime = 0;
                    if (m_isPeeking)
                    {
                        GameplaySystem.cinema.ApplyLookAhead(LookAhead.None);
                        m_isPeeking = false;
                    }
                    break;
                case 1:
                    if (m_isPeeking == false)
                    {
                        EvaluatePeek(LookAhead.Up);
                    }
                    break;
                case -1:
                    if (m_isPeeking == false)
                    {
                        EvaluatePeek(LookAhead.Down);
                    }
                    break;
            }
        }

        private void EvaluatePeek(LookAhead peek)
        {
            m_holdTime += Time.deltaTime;
            if (m_holdTime >= m_holdForPeek)
            {
                GameplaySystem.cinema.ApplyLookAhead(peek);
                m_isPeeking = true;
            }
        }
    }
}