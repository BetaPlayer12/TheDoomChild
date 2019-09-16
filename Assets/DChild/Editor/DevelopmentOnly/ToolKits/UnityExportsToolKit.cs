using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityExportsToolKit
{
    public void Export(RenderTexture renderTexture, TextureFormat textureFormat, string filePath, string fileName)
    {
        Texture2D virtualPhoto =
            new Texture2D(renderTexture.width, renderTexture.height, textureFormat, false);
        // false, meaning no need for mipmaps
        virtualPhoto.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(
            $"{filePath}/{filePath}.png", bytes);
    }
}
