
using DChild;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildEditor.Utility
{
    [ExecuteAlways]
    public class CameraRenderingAnalyzer_Window : OdinEditorWindow
    {
        [MenuItem("Tools/DChild Utility/Camera Rendering Analyzer Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<CameraRenderingAnalyzer_Window>(false, "Camera Rendering Analyzer", true);
        }

        [SerializeField, OnValueChanged("OnCustomCameraChange")]
        private bool m_useCustomCamera;
        [SerializeField, ShowIf("m_useCustomCamera")]
        private Camera m_camera;

        [SerializeField]
        private bool m_useSortLayer;
        [SerializeField,SortingLayer, ShowIf("m_useSortLayer")]
        private int m_selectedLayer;

        [SerializeField, TableList(AlwaysExpanded = true, IsReadOnly = true), TabGroup("Sprite Count")]
        private CameraRenderAnalyzer.Info[] m_results;
        [SerializeField, DisplayAsString, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, IsReadOnly = true, Expanded = true), TabGroup("Sorting Layers")]
        private List<string> m_sortingLayers;
        [SerializeField, TableList(AlwaysExpanded = true), TabGroup("Sprite Breakdown")]
        private SceneSpriteBreakdown.SpriteBreakDown[] m_spriteBreakdown;

        [SerializeField, HideInInspector]
        private CameraRenderAnalyzer m_instance;
        private bool m_stateChange;

        [SerializeField, HideInInspector]
        private SceneSpriteBreakdown m_breakdownInstance;

        private PlayModeStateChange m_currentChangeState;
        private bool allowUpdate => m_currentChangeState != PlayModeStateChange.ExitingEditMode && m_currentChangeState != PlayModeStateChange.ExitingPlayMode;

        protected override void Initialize()
        {
            base.Initialize();
            m_camera = Camera.main;
            m_instance = new CameraRenderAnalyzer(Camera.main);
            m_breakdownInstance = new SceneSpriteBreakdown(Camera.main);
            m_sortingLayers = m_instance.sortingLayers;
            m_instance.RecordRenderers();
            m_breakdownInstance.GetSpriteRenderers();
            m_results = m_instance.AnalyzeShownSprites();
            m_spriteBreakdown = m_breakdownInstance.AnalyzedObjectRenderers();
            m_breakdownInstance.SetSortingLayer(m_selectedLayer, m_useSortLayer);
            EditorApplication.playModeStateChanged -= OnPlayModeChange;
            EditorSceneManager.sceneClosed -= OnSceneClosed;
            EditorSceneManager.sceneOpened -= OnSceneOpen;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChange;
            EditorSceneManager.sceneClosed += OnSceneClosed;
            EditorSceneManager.sceneOpened += OnSceneOpen;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private void OnPlayModeChange(PlayModeStateChange obj)
        {
            m_currentChangeState = obj;
        }

        private void OnCustomCameraChange()
        {
            if (m_useCustomCamera)
            {
                //Nah
            }
            else
            {
                m_camera = Camera.main;
            }
            m_instance.SetCamera(m_camera);
            m_breakdownInstance.SetCamera(m_camera);
        }

        private void OnHierarchyChanged()
        {
            if (m_useCustomCamera == false && m_camera == null)
            {
                m_camera = Camera.main;
                m_instance.SetCamera(m_camera);
                m_breakdownInstance.SetCamera(m_camera);
            }
            m_instance.RecordRenderers();
            m_breakdownInstance.GetSpriteRenderers();
        }

        private void OnSceneOpen(Scene scene, OpenSceneMode mode)
        {
            UpdateAnalysis(true);
        }

        private void OnSceneClosed(Scene scene)
        {
            if (allowUpdate)
            {
                m_instance.RecordRenderers();
                m_breakdownInstance.GetSpriteRenderers();
            }
        }

        private void OnSceneUnloaded(Scene arg0)
        {
            if (allowUpdate)
            {
                m_instance.RecordRenderers();
                m_breakdownInstance.GetSpriteRenderers();
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            UpdateAnalysis(true);
        }

        private void UpdateAnalysis(bool updateCamera)
        {
            if (allowUpdate)
            {
                if (updateCamera && m_useCustomCamera == false && m_camera == null)
                {
                    m_camera = Camera.main;
                    m_instance.SetCamera(m_camera);
                    m_breakdownInstance.SetCamera(m_camera);
                }

                m_instance.RecordRenderers();
                m_breakdownInstance.GetSpriteRenderers();
                m_results = m_instance.AnalyzeShownSprites();
                m_spriteBreakdown = m_breakdownInstance.AnalyzedObjectRenderers();

            }
        }

        protected override void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChange;
            EditorSceneManager.sceneClosed -= OnSceneClosed;
            EditorSceneManager.sceneOpened -= OnSceneOpen;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            base.OnDestroy();
        }

        private void Update()
        {
            m_results = m_instance.AnalyzeShownSprites();
            m_spriteBreakdown = m_breakdownInstance.AnalyzedObjectRenderers();
            m_breakdownInstance.SetSortingLayer(m_selectedLayer, m_useSortLayer);
            Repaint();
        }
    }
}