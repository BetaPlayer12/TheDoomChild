using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ModifySoul : MonoBehaviour
    {
        [SerializeField]
        public int m_soul = 999999;
        [Button]
        private void AddSouls()
        {
            GameplaySystem.playerManager.player.inventory.AddSoulEssence(m_soul);
        }
        [Button]
        private void DeductSouls()
        {
            GameplaySystem.playerManager.player.inventory.AddSoulEssence(-m_soul);
        }
        [Button]
        private void EmptySouls()
        {
            GameplaySystem.playerManager.player.inventory.SetSoulEssence(0);
        }
    }
}
