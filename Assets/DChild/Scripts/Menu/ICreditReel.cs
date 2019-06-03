using Holysoft.Event;

namespace DChild.Menu
{
    public interface ICreditReel
    {
        event EventAction<EventActionArgs> CreditsPlay;
        event EventAction<EventActionArgs> CreditsPause;
        event EventAction<EventActionArgs> CreditsStop;
    }
}