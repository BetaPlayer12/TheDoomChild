namespace Holysoft.Collections
{
    [System.Serializable]
    public class CountupTimer : ITimer
    {
        private float m_time;

        public float time => m_time;

        public void Reset() => m_time = 0f;

        public void Tick(float deltaTime) => m_time += deltaTime;
    }

}