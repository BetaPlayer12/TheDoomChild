using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class PlayerArmyProgressionSaveData
    {
        [SerializeField]
        private int[] m_unlockedCharacters;
    }
}