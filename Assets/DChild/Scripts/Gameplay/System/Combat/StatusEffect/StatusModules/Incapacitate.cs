using Holysoft.Event;
using System;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public class Incapacitate : IStatusEffectModule
    {
        public IStatusEffectModule GetInstance() => this;

        public void Start(Character character)
        {
             var controller = character.GetComponent<IController>();
            if (controller != null)
            {
                controller.Disable();
                controller.ControllerStateChange += OnControllerStateChange;
            }
        }

        public void Stop(Character character)
        {
            var controller = character.GetComponent<IController>();
            if (controller != null)
            {
                controller.ControllerStateChange -= OnControllerStateChange;
                controller.Enable();
            }
        }

        private void OnControllerStateChange(object sender, EventActionArgs<bool> eventArgs)
        {
            if (eventArgs.info == true)
            {
                //Force Disable Controller if something else enables this
                var controller = (IController)sender;
                controller.Disable();
            }
        }
    }
}