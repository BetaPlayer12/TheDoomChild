using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public class RendererTimeHandler
    {
        private float m_timeScale;
        private List<Material> m_materials;
        private List<int> m_count;

        private List<Material> m_checkReference;
#if UNITY_EDITOR
        public int registeredObjectCount => m_materials.Count;
#endif

        public RendererTimeHandler(float timeScale)
        {
            this.m_timeScale = timeScale;
            m_materials = new List<Material>();
            m_count = new List<int>();
            m_checkReference = new List<Material>();
        }

        public static void AlignTime(Material[] materials, float timeScale)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_TimeScale", timeScale);
            }
        }

        public void Register(IRendererObjects renderList)
        {
            m_checkReference.AddRange(m_materials);
            var materials = renderList.materials;
            var countPerMaterials = renderList.countPerMaterial;

            for (int i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
                if (m_checkReference.Contains(material))
                {
                    for (int j = 0; j < m_checkReference.Count; j++)
                    {
                        if (m_checkReference[j] == material)
                        {
                            m_count[j] += countPerMaterials[i];
                            m_checkReference.RemoveAt(j);
                            break;
                        }
                    }
                }
                else
                {
                    material.SetFloat("_TimeScale", m_timeScale);
                    m_materials.Add(material);
                    m_count.Add(countPerMaterials[i]);
                }
            }
        }

        public void Unregister(IRendererObjects renderList)
        {
            m_checkReference.AddRange(m_materials);
            var toDeleteMaterials = renderList.materials;
            var toDeleteCount = renderList.countPerMaterial;

            Debug.Log(toDeleteMaterials.Length);
            for (int i = 0; i < toDeleteMaterials.Length; i++)
            {
                var material = toDeleteMaterials[i];
                if (m_checkReference.Contains(material))
                {
                    for (int j = 0; j < m_checkReference.Count; j++)
                    {
                        if (m_checkReference[j] == material)
                        {
                            m_count[j] -= toDeleteCount[i];
                            if (m_count[j] <= 0)
                            {
                                material.SetFloat("_TimeScale", 1f);
                                m_materials.RemoveAt(j);
                                m_count.RemoveAt(j);
                            }
                            m_checkReference.RemoveAt(j);
                            break;
                        }
                    }
                }
            }
        }

        
        public void AlignTime(float timeScale)
        {
            m_timeScale = timeScale;
            for (int i = 0; i < m_materials.Count; i++)
            {
                m_materials[i].SetFloat("_TimeScale", m_timeScale);
            }
        }

        public void ClearNull() => m_materials = m_materials.Where(item => item != null).ToList();
    }
}