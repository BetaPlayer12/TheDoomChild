using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment.Item
{
    [CustomEditor(typeof(OneWayPortal))]
    public class OneWayPortal_Inspector : OdinEditor
    {
        private OneWayPortal m_portal;
        private SerializedProperty m_destinationProp;
        private SerializedProperty m_instantiatedProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_portal = (OneWayPortal)target;
            m_destinationProp = serializedObject.FindProperty(Convention.PORTAL_ONEWAY_DESTINATION_VARNAME);
            m_instantiatedProp = serializedObject.FindProperty(Convention.PORTAL_ONEWAY_INSTANTIATED_VARNAME);
        }

        private void OnSceneGUI()
        {
            serializedObject.Update();

            if (m_instantiatedProp.boolValue)
            {
                Handles.color = Color.yellow;
                m_destinationProp.vector3Value = Handles.FreeMoveHandle(m_destinationProp.vector3Value, Quaternion.identity, 0.2f, Vector3.one * 0.01f, Handles.CubeHandleCap);
                Handles.Label(m_destinationProp.vector3Value, $"{m_portal.gameObject.name}\nDestination");
                var position = m_portal.transform.position;
                Handles.DrawLine(position, m_destinationProp.vector3Value);
            }
            else
            {
                m_destinationProp.vector3Value = m_portal.transform.position + Vector3.right;
                m_instantiatedProp.boolValue = true;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}