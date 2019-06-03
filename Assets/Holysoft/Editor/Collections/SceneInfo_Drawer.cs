using Holysoft.Collections;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;


namespace HolysoftEditor.Collections
{
    public class SceneInfo_Drawer : OdinValueDrawer<SceneInfo>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var sceneInfo = ValueEntry.SmartValue;
            SceneAsset m_sceneAsset = m_sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneInfo.scenePath);
            EditorGUI.BeginChangeCheck();
            m_sceneAsset = EditorGUILayout.ObjectField("Scene", m_sceneAsset, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                sceneInfo.sceneName = m_sceneAsset.name;
                var newPath = AssetDatabase.GetAssetPath(m_sceneAsset);
                sceneInfo.scenePath = newPath;
                ValueEntry.SmartValue = sceneInfo;
            }
        }
    }
}