using Holysoft.Collections;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
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
            m_sceneAsset = EditorGUILayout.ObjectField(label, m_sceneAsset, typeof(SceneAsset), false) as SceneAsset;
            if (EditorGUI.EndChangeCheck())
            {
                UpdateSceneInfo(sceneInfo, m_sceneAsset);
            }

            if (GUILayout.Button("Update Scene Info"))
            {
                UpdateSceneInfo(sceneInfo, m_sceneAsset);
            }
        }

        private void UpdateSceneInfo(SceneInfo sceneInfo, SceneAsset m_sceneAsset)
        {
            sceneInfo.sceneName = m_sceneAsset.name;
            var newPath = AssetDatabase.GetAssetPath(m_sceneAsset);
            sceneInfo.scenePath = newPath;

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetEntry entry = settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_sceneAsset)));
            sceneInfo.isAddressables = entry != null;

            ValueEntry.SmartValue = sceneInfo;
        }
    }
}