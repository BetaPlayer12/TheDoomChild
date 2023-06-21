using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "CombatArtList", menuName = "DChild/Database/Combat Art List")]
    public class CombatArtList : SerializedScriptableObject
    {
        [SerializeField,AssetSelector(IsUniqueList =true)]
        private CombatArtData[] m_combatArts;

        public CombatArtData GetCombatArtData(CombatArt combatArt)
        {
            for (int i = 0; i < m_combatArts.Length; i++)
            {
                var art = m_combatArts[i];
                if(art.connectedCombatArt == combatArt)
                {
                    return art;
                }
            }
            return null;
        }
    }
}