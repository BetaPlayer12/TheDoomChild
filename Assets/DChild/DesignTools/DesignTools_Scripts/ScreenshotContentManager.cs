using UnityEngine;
using Sirenix.OdinInspector;
using DChildEditor.DesignTool.LevelMap;

namespace DChildEditor.DesignTool.LevelMap.Screenshot
{
    public class ScreenshotContentManager : MonoBehaviour
    {
        [SerializeField, ChildGameObjectsOnly]
        private Transform[] m_contents;

        public void InitializeContent()
        {
            transform.parent?.GetComponentInParent<ScreenshotContentManager>()?.InitializeContent();
            for (int i = 0; i < m_contents.Length; i++)
            {
                m_contents[i].gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }
    }

}