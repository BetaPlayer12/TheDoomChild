using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "PrimarySkillList", menuName = "DChild/Database/Primary Skill List")]
    public class PrimarySkillList : ScriptableObject
    {
        [SerializeField,AssetList]
        private PrimarySkillData[] m_datas;

        public int Count => m_datas.Length;

        public PrimarySkillData GetData(int index) => m_datas[index];
    }
}
