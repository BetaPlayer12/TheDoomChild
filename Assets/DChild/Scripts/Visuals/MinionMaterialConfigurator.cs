using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMaterialConfigurator : MonoBehaviour
{

    [SerializeField]
    private List<MaterialChanges> m_materialChangeList = new List<MaterialChanges>();

    [Serializable]
    public class MaterialChanges
    {
        private MaterialPropertyBlock m_materialBlock;

        [SerializeField]
        private List<GameObject> m_targetObject = new List<GameObject>();

        [SerializeField]
        private float m_texHighlight;
        [SerializeField]
        private Color m_overAllColor;

        public void StartMaterialChange()
        {
            for (int x = 0; x < m_targetObject.Count; x++)
            {
                m_materialBlock = new MaterialPropertyBlock();
                var currentMinion = m_targetObject[x].GetComponentInChildren<MeshRenderer>();
                currentMinion.GetPropertyBlock(m_materialBlock);
                m_materialBlock.SetFloat("Vector1_CutomTexHighlight", m_texHighlight);
                m_materialBlock.SetColor("Color_OverallTexColor", m_overAllColor);
                currentMinion.SetPropertyBlock(m_materialBlock);
            }
        }

    }

    void Start()
    {
        for(int x = 0; x < m_materialChangeList.Count; x++)
        {
            m_materialChangeList[x].StartMaterialChange();
        }

    }

}
