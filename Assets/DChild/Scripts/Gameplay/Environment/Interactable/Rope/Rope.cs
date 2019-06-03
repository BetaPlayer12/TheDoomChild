using System.Collections.Generic;
using Holysoft;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Item
{
    [RequireComponent(typeof(Rigidbody2D), typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class Rope : Actor
    {
        [SerializeField]
        [HideInInspector]
        private LineRenderer m_lineRenderer;
        [SerializeField]
        [HideInInspector]
        private List<GameObject> m_segmentList;

        private void Update()
        {
            m_lineRenderer.SetPosition(0, Vector3.zero);
            for (int i = 0; i < m_segmentList.Count; i++)
            {
                m_lineRenderer.SetPosition(i + 1, m_segmentList[i].transform.localPosition);
            }
        }

        #region Editor Only

#if UNITY_EDITOR
        [SerializeField]
        private Material m_material;
        [SerializeField]
        [MinValue(0.1f)]
        private float m_width = 1f;
        [SerializeField]
        [MinValue(0.1f)]
        private float m_height = 20f;
        [SerializeField]
        [MinValue(1)]
        private int m_segmentCount = 5;
        [SerializeField]
        [MinValue(0f)]
        private float m_angularDrag = 5f;
        [SerializeField]
        [MinValue(0f)]
        [MaxValue(180f)]
        private float m_angleLimit = 75f;

        public List<GameObject> segmentList => m_segmentList;
        public LineRenderer lineRenderer => m_lineRenderer;

        private void OnValidate()
        {
            if (m_segmentList == null)
            {
                m_segmentList = new List<GameObject>();
            }
            //Adjust Visuals
            m_lineRenderer = GetComponent<LineRenderer>();

            var ropeRigidbody = GetComponent<Rigidbody2D>();
            ropeRigidbody.bodyType = RigidbodyType2D.Static;

            //Update Renderer
            m_lineRenderer.material = m_material;
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.positionCount = m_segmentCount + 1;
            m_lineRenderer.startWidth = m_width;
            m_lineRenderer.endWidth = m_width;
            m_lineRenderer.SetPosition(0, Vector3.zero);
        }
#endif 
        #endregion
    }
}

#if UNITY_EDITOR
namespace DChildEditor
{
    public static partial class Convention
    {
        public const string ROPE_MATERIAL_VARNAME = "m_material";
        public const string ROPE_WIDTH_VARNAME = "m_width";
        public const string ROPE_HEIGHT_VARNAME = "m_height";
        public const string ROPE_SEGMENTCOUNT_VARNAME = "m_segmentCount";
        public const string ROPE_ANGLEDRAG_VARNAME = "m_angularDrag";
        public const string ROPE_ANGLELIMIT_VARNAME = "m_angleLimit";
    }
}
#endif