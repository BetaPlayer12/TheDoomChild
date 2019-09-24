using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnityExportsToolKit
{
    private static UnityExportsWindow m_window;

    [MenuItem("Tools/Kit/Export/Open Window")]
    private static void OpenWindow()
    {
        m_window = EditorWindow.GetWindow<UnityExportsWindow>();
        m_window.Show();
        m_window.InitializeWindow();
    }

    public static void Export(RenderTexture renderTexture, TextureFormat textureFormat, string filePath, string fileName)
    {
        RenderTexture.active = renderTexture;
        Texture2D virtualPhoto =
            new Texture2D(renderTexture.width, renderTexture.height, textureFormat, false);
        // false, meaning no need for mipmaps
        virtualPhoto.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        //virtualPhoto.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(
            $"{filePath}/{fileName}.png", bytes);
    }
}

