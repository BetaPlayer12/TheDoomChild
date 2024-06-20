using Holysoft.Event;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerControlledObject : MonoBehaviour, IController
    {
        private IPlayer m_owner;

        public IPlayer owner { get => m_owner; }

        public event EventAction<EventActionArgs<bool>> ControllerStateChange;

        public void Disable()
        {
            m_owner.controller.Disable();
        }

        public void Enable()
        {
            m_owner.controller.Enable();
        }

        public void SetOwner(IPlayer player)
        {
           if (m_owner != null)
            {
                m_owner.controller.ControllerDisabled -= OnOwnerControllerDisabled;
                m_owner.controller.ControllerEnabled -= OnOwnerControllerEnabled;
            }
            m_owner = player;

            m_owner.controller.ControllerDisabled += OnOwnerControllerDisabled;
            m_owner.controller.ControllerEnabled += OnOwnerControllerEnabled;
        }

        private void OnOwnerControllerEnabled(object sender, EventActionArgs eventArgs)
        {
            var eventCache = new EventActionArgs<bool>();
            eventCache.Set(true);
            ControllerStateChange?.Invoke(this, eventCache);
        }

        private void OnOwnerControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            var eventCache = new EventActionArgs<bool>();
            eventCache.Set(false);
            ControllerStateChange?.Invoke(this, eventCache);
        }
    }
}