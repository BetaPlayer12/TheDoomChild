using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu
{
    [CreateAssetMenu(fileName = "LocationImageList", menuName = "DChild/Location Image")]
    public class LocationImageList : SerializedScriptableObject
    {
        [SerializeField, HideReferenceObjectPicker]
        private Dictionary<Location, Sprite> m_list = new Dictionary<Location, Sprite>();

        public Sprite GetImageOf(Location location) => m_list.ContainsKey(location) ? m_list[location] : null;
    }
}