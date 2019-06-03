namespace Holysoft
{
    /// <summary>
    /// Singleton for Non-MonoBehaviour classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        private static T m_instance;

        public Singleton()
        {
            if (m_instance == null)
            {
                m_instance = currentInstance;
            }
        }

        protected abstract T currentInstance { get; }

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new T();
                }

                return m_instance;
            }
        }
    }
}