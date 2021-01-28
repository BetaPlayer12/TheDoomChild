using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using Doozy.Engine;
using Holysoft.Event;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Holysoft.UI;
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.UI.Map
{
    public class MapLocationButton : MonoBehaviour
    {
        [SerializeField, HideInPrefabAssets]
        private LocationData m_location;
        [SerializeField]
        private UIHighlight m_playerIndicator;
        [SerializeField]
        private UIHighlight m_availabilityIndicator;
        [SerializeField]
        private WorldMapHandler m_worldMap; //Delete This as this is just a temp Hack to track the player

        private bool m_indicatorHighlighted;

        public LocationData locationData => m_location;
        public Location location => m_location.location;

        public void HighlightPlayerIndicator(bool highlight)
        {
            m_indicatorHighlighted = highlight;
            HighlightIndicator(m_playerIndicator, highlight);
        }

        public void HighlightAvailabilityIndicator(bool highlight)
        {
            HighlightIndicator(m_availabilityIndicator, highlight);
        }

        public string GetTransferMessage()
        {
            var locationName = $"{m_location.location.ToString().Replace("_", " ")} \n";
            var message = m_indicatorHighlighted ? "Travel back to Location" : "Travel to Location";
            return locationName + message;
        }

        private void HighlightIndicator(UIHighlight indicator, bool highlight)
        {
            if (highlight)
            {
                indicator.UseHighlightState();
            }
            else
            {
                indicator.UseNormalizeState();
            }
        }

        private void OnValidate()
        {
            if (m_location != null)
            {
                gameObject.name = "TravelButton - " + m_location.location.ToString();
            }
        }
    }
}