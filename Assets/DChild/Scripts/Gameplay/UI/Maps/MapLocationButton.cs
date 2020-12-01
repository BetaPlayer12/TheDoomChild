using DChild.Gameplay.Systems.Serialization;
using Doozy.Engine;
using Holysoft.Event;
using System;
using UnityEngine;

namespace DChild.Gameplay.UI.Map
{
    public class MapLocationButton : MonoBehaviour
    {
        [SerializeField]
        private LocationData m_location;

        public void GoHere()
        {
            GameSystem.RequestConfirmation(OnAccept, $"Travel to {m_location.location.ToString()}");
        }

        private void OnAccept(object sender, EventActionArgs eventArgs)
        {
            GameplaySystem.playerManager.player.character.transform.position = m_location.position;
            GameSystem.LoadZone(m_location.scene,true);
            GameEventMessage.SendEvent("Location Transfer");
        }
    }
}