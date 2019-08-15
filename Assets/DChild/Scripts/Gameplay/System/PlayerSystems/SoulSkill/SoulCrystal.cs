using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [CreateAssetMenu(fileName = "SoulCrystal", menuName = "DChild/Database/Soul Crystal")]
    public class SoulCrystal : SerializedScriptableObject
    {
        [SerializeField, OnValueChanged("UpdateReference")]
        private SoulSkill m_activateSoulSkill;
        [SerializeField, ReadOnly]
        private string m_name;
        [SerializeField]
        private Sprite m_icon;
        [SerializeField]
        private Challenge[] m_challenges;

        public Sprite icon => m_icon;
        public string crystalName => m_name;
        public string challengeInfo
        {
            get
            {
                string message = "";
                for (int i = 0; i < m_challenges.Length; i++)
                {
                    message += m_challenges[i].message;
                }
                return message;
            }
        }

        public bool IsQualifiedToActiveSkill()
        {
            for (int i = 0; i < m_challenges.Length; i++)
            {
                if (m_challenges[i].IsComplete() == false)
                {
                    return false;
                }
            }
            return true;
        }

        public void ActivateSoulSkill()
        {

        }

#if UNITY_EDITOR
        private void UpdateReference()
        {
            if (m_activateSoulSkill == null)
            {
                m_name = "Unknown Crystal";
            }
            else
            {
                m_name = m_activateSoulSkill.name + " Crystal";
            }
            UpdateAssetName();

        }

        private void UpdateAssetName()
        {
            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            var fileName = m_name.Replace(" ", string.Empty);
            FileUtility.RenameAsset(this, assetPath, fileName);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}