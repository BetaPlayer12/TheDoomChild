using Holysoft.Event;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class StatusEffectAnimationHandler : StatusEffectComponent
    {
        protected override void OnEffectEnd(object sender, StatusEffectEventArgs eventArgs)
        {

        }

        protected override void OnEffectStart(object sender, StatusEffectEventArgs eventArgs)
        {
            eventArgs.reciever.GetComponentInChildren<IStatusRecieverAnimation>().SetStatusEffectAnimation("");
        }
    }
}