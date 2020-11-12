using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "PrimarySkillData", menuName = "DChild/Database/Primary Skill Data")]
    public class PrimarySkillData : ScriptableObject
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private string m_description;
        [SerializeField, OnValueChanged("SkillChanged")]
        private PrimarySkill m_skill;
        [SerializeField, PreviewField]
        private Sprite m_border;
        [SerializeField, PreviewField]
        private Sprite m_icon;

        public string skillName => m_name;
        public string description => m_description;
        public PrimarySkill skill => m_skill;
        public Sprite border => m_border;
        public Sprite icon => m_icon;

#if UNITY_EDITOR
        private void SkillChanged()
        {
            m_name = m_skill.ToString();
           var path =  AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path, m_name + "SkillData");
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
