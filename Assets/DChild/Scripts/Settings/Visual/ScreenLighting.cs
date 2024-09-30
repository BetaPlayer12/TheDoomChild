using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScreenLighting : MonoBehaviour
{
    #region Variables
    public Shader curShader;
    [SerializeField]
    [Range(0, 1.5f)]
    private float m_brightness = 1.0f;
    [SerializeField]
    [Range(0, 2f)]
    public float m_saturation = 1.0f;
    [SerializeField]
    [Range(0, 3f)]
    public float m_contrast = 1.0f;
    [Range(0, 1f)]
    private bool m_bloom = true;
    private Material curMaterial;
    #endregion

    public float brightness
    {
        get
        {
            return m_brightness;
        }

        set
        {
            m_brightness = Mathf.Clamp(value, -1, 1f);
        }
    }
    public float saturation
    {
        get
        {
            return m_saturation;
        }

        set
        {
            m_saturation = Mathf.Clamp(value, 0, 2f);
        }
    }
    public float contrast
    {
        get
        {
            return m_contrast;
        }

        set
        {
            m_contrast = Mathf.Clamp(value, 0, 3f);
        }
    }

    public bool bloom
    {
        get => m_bloom;

        set => m_bloom = value;
    }

    #region Properties
    Material material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }
    #endregion
    // Use this for initialization
    void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (curShader != null)
        {
            material.SetFloat("_BrightnessAmount", brightness);
            material.SetFloat("_SaturationAmount", m_saturation);
            material.SetFloat("_ContrastAmount", m_contrast);
            Graphics.Blit(sourceTexture, destTexture, material);
        }
        else
        {
            Graphics.Blit(sourceTexture, destTexture);
        }


    }

    void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }

    }

#if UNITY_EDITOR
    public void Initialize(Shader shader)
    {
        curShader = shader;
    }
#endif
}