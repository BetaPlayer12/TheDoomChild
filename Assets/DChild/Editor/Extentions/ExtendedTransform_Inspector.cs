﻿
using UnityEngine;
using UnityEditor;

namespace HolysoftEditor
{
    [CustomEditor(typeof(Transform))]
    [CanEditMultipleObjects]
    public class ExtendedTransform_Inspector : Editor
    {
        private Transform _transform;

        public override void OnInspectorGUI()
        {
            //We need this for all OnInspectorGUI sub methods
            _transform = (Transform)target;

            StandardTransformInspector();

            GUI.enabled = false;
            EditorGUILayout.Vector3Field("World Position", _transform.position);
            EditorGUILayout.Vector3Field("World Rotation", _transform.rotation.eulerAngles);
            GUI.enabled = true;
        }

        private void StandardTransformInspector()
        {
            bool didPositionChange = false;
            bool didRotationChange = false;
            bool didScaleChange = false;

            // Watch for changes.
            //  1)  Float values are imprecise, so floating point error may cause changes
            //      when you've not actually made a change.
            //  2)  This allows us to also record an undo point properly since we're only
            //      recording when something has changed.

            // Store current values for checking later
            Vector3 initialLocalPosition = _transform.localPosition;
            Vector3 initialLocalEuler = _transform.localEulerAngles;
            Vector3 initialLocalScale = _transform.localScale;

            EditorGUI.BeginChangeCheck();
            Vector3 localPosition = EditorGUILayout.Vector3Field("Position", _transform.localPosition);
            if (EditorGUI.EndChangeCheck())
                didPositionChange = true;

            EditorGUI.BeginChangeCheck();
            Vector3 localEulerAngles = EditorGUILayout.Vector3Field("Euler Rotation", _transform.localEulerAngles);
            if (EditorGUI.EndChangeCheck())
                didRotationChange = true;

            EditorGUI.BeginChangeCheck();
            Vector3 localScale = EditorGUILayout.Vector3Field("Scale", _transform.localScale);
            if (EditorGUI.EndChangeCheck())
                didScaleChange = true;

            // Apply changes with record undo
            if (didPositionChange || didRotationChange || didScaleChange)
            {
                Undo.RecordObject(_transform, _transform.name);

                if (didPositionChange)
                    _transform.localPosition = localPosition;

                if (didRotationChange)
                    _transform.localEulerAngles = localEulerAngles;

                if (didScaleChange)
                    _transform.localScale = localScale;

            }

            // Since BeginChangeCheck only works on the selected object
            // we need to manually apply transform changes to all selected objects.
            Transform[] selectedTransforms = Selection.transforms;
            if (selectedTransforms.Length > 1)
            {
                foreach (var item in selectedTransforms)
                {
                    if (didPositionChange || didRotationChange || didScaleChange)
                        Undo.RecordObject(item, item.name);

                    if (didPositionChange)
                    {
                        item.localPosition = ApplyChangesOnly(
                            item.localPosition, initialLocalPosition, _transform.localPosition);
                    }

                    if (didRotationChange)
                    {
                        item.localEulerAngles = ApplyChangesOnly(
                            item.localEulerAngles, initialLocalEuler, _transform.localEulerAngles);
                    }

                    if (didScaleChange)
                    {
                        item.localScale = ApplyChangesOnly(
                            item.localScale, initialLocalScale, _transform.localScale);
                    }

                }
            }
        }

        private Vector3 ApplyChangesOnly(Vector3 toApply, Vector3 initial, Vector3 changed)
        {
            if (!Mathf.Approximately(initial.x, changed.x))
                toApply.x = _transform.localPosition.x;

            if (!Mathf.Approximately(initial.y, changed.y))
                toApply.y = _transform.localPosition.y;

            if (!Mathf.Approximately(initial.z, changed.z))
                toApply.z = _transform.localPosition.z;

            return toApply;
        }
    }
}