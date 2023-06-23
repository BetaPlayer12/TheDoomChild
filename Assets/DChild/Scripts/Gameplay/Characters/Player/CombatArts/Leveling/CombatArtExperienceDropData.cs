using UnityEngine;

namespace DChild.Gameplay.Characters.Player.CombatArt.Leveling
{
    [CreateAssetMenu(fileName = "CombatArtExperienceDropData", menuName = "DChild/Gameplay/Combat/Combat Art EXP Drop Data")]
    public class CombatArtExperienceDropData : ScriptableObject
    {
        [SerializeField, Min(1)]
        private int m_exp =1;

        public int exp => m_exp;
    }

}