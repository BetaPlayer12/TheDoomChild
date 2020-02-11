using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class UnityExportsWindow : OdinEditorWindow
{
    private static UnityExportsWindow m_instance;

    private RenderTexture m_toExport;
    private string m_folderPath;
    private string m_fileName;
    private TextureFormat m_textureFormat;

    public void InitializeWindow()
    {
        titleContent = new GUIContent("Spine Utility Window", EditorIcons.Male.Active);
    }

    private void DrawMain()
    {
        m_toExport = (RenderTexture)SirenixEditorFields.UnityObjectField(m_toExport, typeof(RenderTexture), false);
        m_folderPath = SirenixEditorFields.FolderPathField(m_folderPath, null, true, false, null);
        m_fileName = SirenixEditorFields.TextField(m_fileName);
        m_textureFormat = (TextureFormat)SirenixEditorFields.EnumDropdown(m_textureFormat);
        if (GUILayout.Button("Export"))
        {
            UnityExportsToolKit.Export(m_toExport, m_textureFormat, m_folderPath, m_fileName);
        }
    }

    protected override void OnGUI()
    {
        DrawMain();
    }

    protected override object GetTarget()
    {
        return m_instance;
    }
}

