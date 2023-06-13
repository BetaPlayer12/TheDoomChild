using UnityEngine;

namespace DChild.Gameplay.Leveling
{
    [CreateAssetMenu(fileName = "ExperiencePointDropData", menuName = "DChild/Gameplay/Combat/EXP Drop Data")]
    public class ExperiencePointDropData : ScriptableObject
    {
        [SerializeField, Min(1)]
        private int m_exp =1;

        public int exp => m_exp;
    }

}