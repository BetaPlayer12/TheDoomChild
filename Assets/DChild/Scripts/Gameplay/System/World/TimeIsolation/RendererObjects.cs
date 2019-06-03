using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    [System.Serializable]
    public class RendererObjects : IRendererObjects
    {
        [SerializeField]
        private Material[] m_materials;
        [SerializeField]
        private int[] m_countPerMaterial;

        public RendererObjects(Material[] materials)
        {
            List<Material> materialList = new List<Material>();
            List<int> countPerMaterialList = new List<int>();
            for (int i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
                if (material.HasProperty("_TimeScale"))
                {
                    if (materialList.Contains(material))
                    {
                        for (int j = 0; j < materialList.Count; j++)
                        {
                            if (materialList[j] == material)
                            {
                                countPerMaterialList[j] += 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        materialList.Add(material);
                        countPerMaterialList.Add(1);
                    }
                }
            }

            m_materials = materialList.ToArray();
            m_countPerMaterial = countPerMaterialList.ToArray();
        }

        public Material[] materials => m_materials;
        public int[] countPerMaterial => m_countPerMaterial;
    }
}