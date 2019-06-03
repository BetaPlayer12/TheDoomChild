namespace Holysoft.Event
{
    public struct StatInfoEventArgs : IEventActionArgs
    {
        public StatInfoEventArgs(int currentValue, int maxValue) : this()
        {
            this.currentValue = currentValue;
            this.maxValue = maxValue;
            percentValue = currentValue / (float)maxValue;
        }

        public int currentValue { get; }
        public int maxValue { get; }
        public float percentValue { get; }
    }
}