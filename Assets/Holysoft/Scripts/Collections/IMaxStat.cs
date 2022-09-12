using Holysoft.Event;

namespace Holysoft.Gameplay
{
    public interface IMaxStat
    {
        int maxValue { get; }
        void SetMaxValue(int value);
        event EventAction<StatInfoEventArgs> MaxValueChanged;
    }
}