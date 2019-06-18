﻿using Holysoft.Event;

namespace Holysoft.Gameplay
{
    public interface ICappedStat : IMaxStat
    {
        int currentValue { get; }
        void AddCurrentValue(int value);
        void ReduceCurrentValue(int value);
        void ResetValueToMax();
        event EventAction<StatInfoEventArgs> ValueChanged;
    }

    public interface IMaxStat
    {
        int maxValue { get; }
        void SetMaxValue(int value);
        event EventAction<StatInfoEventArgs> MaxValueChanged;
    }
}