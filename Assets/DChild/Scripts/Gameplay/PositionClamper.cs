using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class PositionClamper : MonoBehaviour
    {
        [System.Serializable]
        private struct Handle
        {
            [SerializeField]
            private bool m_willClamp;
            [SerializeField, ShowIf("m_willClamp"), HorizontalGroup("Number")]
            private float m_min;
            [SerializeField, ShowIf("m_willClamp"), HorizontalGroup("Number")]
            private float m_max;

            public bool willClamp => m_willClamp;
            public float min => m_min;
            public float max => m_max;
        }

        [SerializeField]
        private Handle m_clampX;
        [SerializeField]
        private Handle m_clampY;

        private float GetClampValue(Handle handle, float value)
        {
            if (handle.willClamp)
            {
                return Mathf.Clamp(value, handle.min, handle.max);
            }
            return value;
        }

        private void LateUpdate()
        {
            var position = transform.position;
            position.x = GetClampValue(m_clampX, position.x);
            position.y = GetClampValue(m_clampY, position.y);
            transform.position = position;
        }
    }
}