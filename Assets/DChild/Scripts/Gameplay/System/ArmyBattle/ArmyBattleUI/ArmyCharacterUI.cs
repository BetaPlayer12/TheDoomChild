using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyCharacterUI : MonoBehaviour
    {
        [Button]
        public void Display(ArmyCharacterData characterData)
        {
            Debug.Log($"Display: {characterData.name}");
        }
    }
}