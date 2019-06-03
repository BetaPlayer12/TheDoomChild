using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public static class SpineToolKit
    {
        private static SpineUtilityWindow m_window;
        private static SkeletonRenderer m_selectedRenderer;

        [MenuItem("Tools/Kit/Spine/Create Utility Window")]
        private static void OpenWindow()
        {
            m_window = EditorWindow.GetWindow<SpineUtilityWindow>();
            m_window.Show();
            m_window.InitializeWindow();
        }

        [MenuItem("Tools/Kit/Spine/Create Hierarchy &h")]
        private static void CreateHierarchy()
        {
            SpineUtility.CreateHierarchyFor(m_selectedRenderer);
        }

        [MenuItem("Tools/Kit/Spine/Create Hierarchy &h", true)]
        private static bool CreateHierarchyValidation()
        {
            m_selectedRenderer = Selection.activeGameObject.GetComponent<SkeletonRenderer>();
            return m_selectedRenderer;
        }

        [MenuItem("Tools/Kit/Spine/Create Hitboxes &%h")]
        private static void CreateHitboxes()
        {
            SpineUtility.CreateHitboxes(m_selectedRenderer);
        }

        [MenuItem("Tools/Kit/Spine/Create Hitboxes &%h", true)]
        private static bool CreateHitboxesValidation()
        {
            m_selectedRenderer = Selection.activeGameObject.GetComponent<SkeletonRenderer>();
            return m_selectedRenderer;
        }

        [MenuItem("Tools/Kit/Spine/Cleaup Hitboxes &%j")]
        private static void CleanupHitboxes()
        {
            m_selectedRenderer = Selection.activeGameObject.GetComponent<SkeletonRenderer>();
            SpineUtility.CleanHitboxHierarchy(m_selectedRenderer);
        }

        [MenuItem("Tools/Kit/Spine/Cleaup Hierarchy &%j", true)]
        private static bool CleanupHitboxesValidation()
        {
            m_selectedRenderer = Selection.activeGameObject.GetComponent<SkeletonRenderer>();
            return m_selectedRenderer;
        }
    }
}