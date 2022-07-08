/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public class DistanceAlphaShift : DistanceShift<SpriteRenderer>
    {
        [SerializeField]
        private SpriteRenderer[] m_renderers;

        protected override SpriteRenderer[] targets => m_renderers;

        protected  override void SetShiftValue(SpriteRenderer renderer, float value)
        {
            var color = renderer.color;
            color.a = value;
            renderer.color = color;
        }
    }
}