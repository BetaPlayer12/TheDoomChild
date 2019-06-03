namespace Holysoft.Event
{
    namespace Collections
    {

        /// <summary>
        /// Tracks the Event Types
        /// </summary>
        public interface IEventTracker
        {
            TrackedEvent[] trackedEvents { get; }

#if UNITY_EDITOR || UNITY_ANDROID
            /// <summary>
            /// Delete potential "empty" data
            /// </summary>
            void CleanData();

            TrackedEvent FindData(int index);
#endif

            void UpdateData();
        }
    }
}