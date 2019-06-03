using UnityEngine;
using Holysoft.Collections;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable]
    public class SafetyModule
    {
        [SerializeField]
        [Tooltip("If the player is in this zone it will run away")]
        private float m_dangerRadius;
        [SerializeField]
        private RangeFloat m_safeRange;
        [SerializeField]
        private bool m_stayOnOneSide;

        private Vector2 m_safeOffset;

        public bool IsTargetTooClose(Vector2 minionPosition, Vector2 targetPosition)
        {
            var distanceFromTarget = Vector2.Distance(minionPosition, targetPosition);
            return distanceFromTarget <= m_dangerRadius;
        }

        public Vector2 GetSafePosition(Vector2 targetPosition) => targetPosition + m_safeOffset;

        public void GenerateNewSafeOffset(Vector2 minionPos, Vector2 targetPosition, Vector2 safeNormal)
        {
            if (m_stayOnOneSide)
            {
                if (minionPos.x > targetPosition.x)
                {
                    safeNormal.x = Mathf.Abs(safeNormal.x);
                }
                else
                {
                    safeNormal.x = -Mathf.Abs(safeNormal.x);
                }
            }
            else
            {
                var xSign = Random.Range(0, 100f) > 50 ? -1 : 1;
                safeNormal.x *= xSign;
            }
            m_safeOffset = safeNormal * m_safeRange.GenerateRandomValue();
        }

#if UNITY_EDITOR
        public float dangerRadius => m_dangerRadius;
        public RangeFloat safeRange => m_safeRange;
#endif
    }
}