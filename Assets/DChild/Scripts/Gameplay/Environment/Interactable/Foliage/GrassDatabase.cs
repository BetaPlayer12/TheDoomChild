using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems.Databases
{
    [CreateAssetMenu(fileName = "GrassDatabase", menuName = "DChild/Database/Grass")]
    public class GrassDatabase : ScriptableObject//, IDatabase
    {
        [SerializeField]
        private Material[] m_materials;

        public Material GetMaterial() => m_materials[Random.Range(0, m_materials.Length)];
    }
}