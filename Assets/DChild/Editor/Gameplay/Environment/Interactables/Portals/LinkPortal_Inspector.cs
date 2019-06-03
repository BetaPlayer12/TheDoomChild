using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment.Item
{
    [CustomEditor(typeof(LinkPortal))]
    public class LinkPortal_Inspector : OdinEditor
    {
        private LinkPortal m_portal;
        private SerializedProperty m_linkTo;
        private SerializedProperty m_exitPositionProp;
        private SerializedProperty m_instantiatedProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_portal = (LinkPortal)target;
            m_linkTo = serializedObject.FindProperty(Convention.PORTAL_LINK_LINKTO_VARNAME);
            m_exitPositionProp = serializedObject.FindProperty(Convention.PORTAL_LINK_EXITPOSITION_VARNAME);
            m_instantiatedProp = serializedObject.FindProperty(Convention.PORTAL_LINK_INSTANTIATED_VARNAME);
        }

        private void OnSceneGUI()
        {
            serializedObject.Update();

            if (m_instantiatedProp.boolValue)
            {
                Handles.color = Color.blue;
                var position = m_portal.transform.position;
                var exitPosition = Handles.FreeMoveHandle(m_exitPositionProp.vector3Value, Quaternion.identity, 0.2f, Vector3.one * 0.01f, Handles.CubeHandleCap);
                //m_exitPositionProp.vector3Value = position + (m_portal.transform.right * Vector3.Distance(exitPosition, m_portal.transform.position));
                m_exitPositionProp.vector3Value = exitPosition;
                Handles.Label(m_exitPositionProp.vector3Value, $"{m_portal.gameObject.name}\nExit");
                Handles.DrawLine(position, m_exitPositionProp.vector3Value);

                if (m_linkTo.objectReferenceValue != null)
                {
                    Handles.color = Color.yellow;
                    var linkedPortal = (Portal)m_linkTo.objectReferenceValue;
                    Handles.Label(linkedPortal.transform.position, $"{m_portal.gameObject.name}\nLinkTo");
                    Handles.DrawLine(position, linkedPortal.transform.position);

                    Handles.color = Color.red;
                    Handles.DrawLine(linkedPortal.transform.position, linkedPortal.destination);
                }
            }
            else
            {
                m_exitPositionProp.vector3Value = m_portal.transform.position + Vector3.right;
                m_instantiatedProp.boolValue = true;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
