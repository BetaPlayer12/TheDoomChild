using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Sirenix.Utilities.Editor;
using DChild.Gameplay.Systems;

namespace DChildEditor.Gameplay
{
    [CustomEditor(typeof(World))]
    public class World_Inspector : OdinEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SirenixEditorGUI.BeginBox("Time");
            EditorGUILayout.LabelField($"Scale: {GameplaySystem.time?.timeScale}");
            EditorGUILayout.LabelField($"Delta Time: {GameplaySystem.time?.deltaTime}");
            EditorGUILayout.LabelField($"Fixed Delta TIme: {GameplaySystem.time?.fixedDeltaTime}");
            SirenixEditorGUI.EndBox();

            //if (Application.isPlaying)
            //{
            //    SirenixEditorGUI.BeginBox("Registered Objects");
            //    EditorGUILayout.LabelField($"Particles: {GameplaySystem.world.registeredParticleCount}");
            //    EditorGUILayout.LabelField($"Renderers: {World.registeredRendererCount}");
            //    EditorGUILayout.LabelField($"Physics: {World.registeredPhysicsCount}");
            //    EditorGUILayout.LabelField($"Spine: {World.registeredSpineCount}");
            //    EditorGUILayout.LabelField($"Isolated Objects: {World.registeredIsolatedObjectCount}");
            //    EditorGUILayout.LabelField($"Isolated Components: {World.registeredIsolatedComponentCount}");
            //    EditorGUILayout.LabelField($"Environment: {World.registeredEnvironmentCount}");
            //    SirenixEditorGUI.EndBox();
            //    Repaint();
            //}
        }

        
    }
}