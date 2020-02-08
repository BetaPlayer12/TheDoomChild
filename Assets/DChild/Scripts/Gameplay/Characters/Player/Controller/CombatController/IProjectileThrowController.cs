using Holysoft.Event;
namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IProjectileThrowController
    {
        event EventAction<EventActionArgs> ProjectileAimCall;
        event EventAction<ControllerEventArgs> ProjectileAimUpdate;
    }
}