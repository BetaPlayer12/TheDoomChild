using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Environment.Item;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment.Interactables
{
    [CustomEditor(typeof(Rope))]
    public class Rope_Inspector : OdinEditor
    {
        private Rope m_rope;
        private SerializedProperty m_materialProp;
        private SerializedProperty m_widthProp;
        private SerializedProperty m_heightProp;
        private SerializedProperty m_segmentCountProp;
        private SerializedProperty m_angularDragProp;
        private SerializedProperty m_angleLimitProp;

        private float m_ropeSegmentDistance;
        private float m_angleDecrement;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_rope = (Rope)target;
            m_materialProp = serializedObject.FindProperty(Convention.ROPE_MATERIAL_VARNAME);
            m_widthProp = serializedObject.FindProperty(Convention.ROPE_WIDTH_VARNAME);
            m_heightProp = serializedObject.FindProperty(Convention.ROPE_HEIGHT_VARNAME);
            m_segmentCountProp = serializedObject.FindProperty(Convention.ROPE_SEGMENTCOUNT_VARNAME);
            m_angularDragProp = serializedObject.FindProperty(Convention.ROPE_ANGLEDRAG_VARNAME);
            m_angleLimitProp = serializedObject.FindProperty(Convention.ROPE_ANGLELIMIT_VARNAME);
        }

        private void OnSceneGUI()
        {
            serializedObject.Update();
            var widthPosition = m_rope.transform.position + (m_rope.transform.right * m_widthProp.floatValue / 2) - (m_rope.transform.up * m_heightProp.floatValue / 2);
            m_widthProp.floatValue = Handles.ScaleSlider(m_widthProp.floatValue, widthPosition, m_rope.transform.right, Quaternion.identity, 0.5f, 0.1f);

            var heightPosition = m_rope.transform.position - (m_rope.transform.up * m_heightProp.floatValue);
            m_heightProp.floatValue = Handles.ScaleSlider(m_heightProp.floatValue, heightPosition, -m_rope.transform.up, Quaternion.identity, 0.5f, 0.01f);
            serializedObject.ApplyModifiedProperties();

            ValidateRope();
        }

        private GameObject CreateRopeSegment(int index, Rigidbody2D connectedTo)
        {
            var ropeSegmentGO = new GameObject("RopeSegment" + index);
            ropeSegmentGO.transform.parent = m_rope.transform;

            //Set Up Rigidbody
            var ropeSegmentRigidbody = ropeSegmentGO.AddComponent<Rigidbody2D>();
            var ropeSegmentHinge = ropeSegmentGO.AddComponent<HingeJoint2D>();
            ropeSegmentHinge.connectedBody = connectedTo;
            var collider = ropeSegmentGO.AddComponent<BoxCollider2D>();
            ropeSegmentGO.AddComponent<RopeSegment>();
            ModifyRopeSegment(ropeSegmentGO, index);
            return ropeSegmentGO;
        }

        private void ModifyRopeSegment(GameObject segment, int index)
        {
            //Set Up HingeJoint
            segment.transform.localPosition = new Vector3(0f, -m_ropeSegmentDistance * (index + 1), 0f);

            var ropeSegmentRigidbody = segment.GetComponent<Rigidbody2D>();
            ropeSegmentRigidbody.angularDrag = m_angularDragProp.floatValue;

            ModifyHingeJoint(segment, index);

            //Setup Collider
            var collider = segment.GetComponent<BoxCollider2D>();
            collider.offset = new Vector2(0, m_ropeSegmentDistance / 2);
            collider.size = new Vector2(m_widthProp.floatValue, m_ropeSegmentDistance);
        }

        private void ModifyHingeJoint(GameObject segment, int index)
        {
            var ropeSegmentHinge = segment.GetComponent<HingeJoint2D>();
            ropeSegmentHinge.autoConfigureConnectedAnchor = false;
            ropeSegmentHinge.anchor = new Vector2(0, m_ropeSegmentDistance);
            ropeSegmentHinge.connectedAnchor = Vector2.zero;
            ropeSegmentHinge.useLimits = true;

            //Limit Hinge Angles
            JointAngleLimits2D limits = new JointAngleLimits2D();
            limits.min = -m_angleLimitProp.floatValue + (m_angleDecrement * index);
            limits.max = m_angleLimitProp.floatValue - (m_angleDecrement * index);
            ropeSegmentHinge.limits = limits;
        }

        private void ValidateRope()
        {
            serializedObject.Update();
            m_ropeSegmentDistance = m_heightProp.floatValue / (float)m_segmentCountProp.intValue;
            m_angleDecrement = m_heightProp.floatValue / (float)m_segmentCountProp.intValue;

            UpdateSegments();
            for (int i = 0; i < m_rope.segmentList.Count; i++)
            {
                ModifyRopeSegment(m_rope.segmentList[i], i);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateSegments()
        {
            //Update Segments
            if (m_segmentCountProp.intValue != m_rope.segmentList.Count)
            {
                if (m_segmentCountProp.intValue < m_rope.segmentList.Count)
                {
                    for (int i = m_rope.segmentList.Count - 1; i >= m_segmentCountProp.intValue; i--)
                    {
                        DestroyImmediate(m_rope.segmentList[i]);
                        m_rope.segmentList.RemoveAt(i);
                    }
                }
                else
                {
                    var currentRigidbody = m_rope.segmentList.Count > 0 ? m_rope.segmentList[m_rope.segmentList.Count - 1].GetComponent<Rigidbody2D>()
                                                                       : m_rope.GetComponent<Rigidbody2D>();
                    for (int i = m_rope.segmentList.Count; i < m_segmentCountProp.intValue; i++)
                    {
                        var segment = CreateRopeSegment(i, currentRigidbody);
                        currentRigidbody = segment.GetComponent<Rigidbody2D>();
                        m_rope.segmentList.Add(segment);
                    }
                }
            }
        }
    }

}