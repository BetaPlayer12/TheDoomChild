﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChildEditor.DesignTool.LevelMap.Screenshot
{
    public class ScreenShotHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_content;
        [SerializeField]
        private Vector2Int m_resolution;
        [SerializeField, FolderPath]
        private string m_filePathDestination;
        [SerializeField]
        private string m_fileName;

        private const string displayName = "Map";

        [Button]
        public void SetForScreenshot()
        {
#if UNITY_EDITOR
            m_content.GetComponentInParent<ScreenshotContentManager>().InitializeContent();
            m_content.SetActive(true);

            var existingSizeIndex = GameViewUtils.FindSize(GameViewSizeGroupType.Standalone, m_resolution.x, m_resolution.y);
            if (existingSizeIndex != -1)
            {
                GameViewUtils.SetSize(existingSizeIndex);
                return;
            }

            existingSizeIndex = GameViewUtils.FindSize(GameViewSizeGroupType.Standalone, displayName);
            if (existingSizeIndex != -1)
            {
                GameViewUtils.RemoveCustomSize(GameViewSizeGroupType.Standalone, displayName);
            }
            GameViewUtils.AddAndSelectCustomSize(GameViewUtils.GameViewSizeType.FixedResolution, GameViewSizeGroupType.Standalone, m_resolution.x, m_resolution.y, displayName);
#endif
        }

        [Button]
        public void CaptureScreen()
        {
            SetForScreenshot();
            ScreenCapture.CaptureScreenshot($"{m_filePathDestination}/{m_fileName}.png");
        }
    }
}