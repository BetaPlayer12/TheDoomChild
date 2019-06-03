using UnityEngine;

namespace DChild.Configurations
{
    [System.Serializable]
    public class GameplaySettings
    {
        public enum Language
        {
            English,
            German,
            Something,
            _COUNT
        }

        public bool showDamageValues;
        public bool showEnemyHealth;
        public Language language;
    }
}