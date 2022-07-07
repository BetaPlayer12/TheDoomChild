using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu.Inputs
{
    [CreateAssetMenu(fileName = "GamepadIconData", menuName = "DChild/Menu/Gamepad Icon Data")]
    public class GamepadIconData : SerializedScriptableObject
    {
        [SerializeField]
        private TMP_SpriteAsset m_spriteAsset;
        [SerializeField]
        private Dictionary<string, int> m_newInputSchemeToIcon;

        public TMP_SpriteAsset spriteAsset => m_spriteAsset;
        public string GetSprite(string input) => $"<sprite = {m_newInputSchemeToIcon[input]}>";
    }

}