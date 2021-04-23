namespace DChild.Gameplay.Systems
{
    public struct TimeInfo : ITime
    {
        public TimeInfo(float timeScale, float deltaTime, float fixedDeltaTime)
        {
            this.timeScale = timeScale;
            this.deltaTime = deltaTime;
            this.fixedDeltaTime = fixedDeltaTime;
        }

        public float timeScale { get; }
        public float deltaTime { get; }
        public float fixedDeltaTime { get; }
    }

}