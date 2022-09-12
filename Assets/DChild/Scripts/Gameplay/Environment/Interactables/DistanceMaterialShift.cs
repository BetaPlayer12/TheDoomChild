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
    public class DistanceMaterialShift : DistanceShift<SpriteRenderer>
    {
        [SerializeField]
        private string m_materialValueReference;
        [SerializeField]
        private SpriteRenderer[] m_renderers;

        private MaterialPropertyBlock m_propertyBlock;
        private int m_materialValueReferenceID;

        protected override SpriteRenderer[] targets => m_renderers;

        protected override void SetShiftValue(SpriteRenderer renderer, float value)
        {
            renderer.GetPropertyBlock(m_propertyBlock);
            m_propertyBlock.SetFloat(m_materialValueReferenceID, value);
            renderer.SetPropertyBlock(m_propertyBlock);
        }

        protected override void Start()
        {
            m_propertyBlock = new MaterialPropertyBlock();
            m_materialValueReferenceID = Shader.PropertyToID(m_materialValueReference);
            base.Start();
        }
    }
}