using DChild.Gameplay.Characters.Players.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialConfigurator : MonoBehaviour
{
    private MaterialPropertyBlock m_materialBlock;
    [SerializeField]
    private GameObject m_targetObject;

    [SerializeField]
    private float m_texHighlight;
    [SerializeField]
    private Color m_overAllColor;

    void Start()
    {
        m_materialBlock = new MaterialPropertyBlock();
        m_targetObject = FindObjectOfType<PlayerDamageable>().gameObject;
        var objectRenderer = m_targetObject.GetComponentInChildren<MeshRenderer>();
        objectRenderer.GetPropertyBlock(m_materialBlock);
        m_materialBlock.SetFloat("Vector1_CutomTexHighlight", m_texHighlight );
        m_materialBlock.SetColor("Color_OverallTexColor", m_overAllColor);
        objectRenderer.SetPropertyBlock(m_materialBlock);
        
    }

}
