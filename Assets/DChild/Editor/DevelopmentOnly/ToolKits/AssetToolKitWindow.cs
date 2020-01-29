using DChildEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor
{
    public class AssetToolKitWindow : OdinEditorWindow
    {

        private static AssetToolKitWindow m_instance;

        [MenuItem("Tools/Kit/Asset ToolKit")]
        private static void OpenWindow()
        {
            m_instance = EditorWindow.GetWindow<AssetToolKitWindow>();
            m_instance.Show();
        }

        [AssetSelector(Paths = "Assets/Testing|Assets/testing|Assets/DChild/Objects/Location", Filter = "t:Texture2D"), SerializeField]
        private Texture2D[] m_textures;
        [SerializeField]
        private string m_prefix;


        [Button]
        private void AddPrefix()
        {
            for (int i = 0; i < m_textures.Length; i++)
            {
                var path = AssetDatabase.GetAssetPath(m_textures[i]);
                var filename = FileUtility.GetFileName(path);
                if (filename.StartsWith("m_prefix") == false)
                {
                    filename = m_prefix + filename;
                    Debug.Log(AssetDatabase.RenameAsset(path, filename + FileUtility.GetAssetExtention(path)));
                }
            }
            AssetDatabase.SaveAssets();
        }
    }

}