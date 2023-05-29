using Holysoft.Event;

namespace Holysoft.Gameplay
{
    public interface IProgressStat : IMaxStat
    {
        int currentValue { get; }
        void AddCurrentValue(int value);
        void ReduceCurrentValue(int value);

        event EventAction<StatInfoEventArgs> ValueChanged;
        event EventAction<EventActionArgs> ReachedFull;
    }
}