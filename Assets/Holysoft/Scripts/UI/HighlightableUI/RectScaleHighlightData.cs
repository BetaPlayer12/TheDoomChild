using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    [CreateAssetMenu(fileName = "RectScaleHighlightData", menuName = "DChild/Menu/Animation Data/Rect Scale")]
    public class RectScaleHighlightData : SwitchHighlightData<Vector3>
    {
        [SerializeField, MinValue(0)]
        private float m_lerpDuration;

        public float lerpDuration => m_lerpDuration;
    }
}