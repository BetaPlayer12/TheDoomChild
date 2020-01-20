using DChild.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    public class EventTimer : SerializedMonoBehaviour
    {
        [SerializeField]
        private bool m_useGameplayTime;
        [SerializeField]
        private IEventTimerHandle m_handle;

        // Start is called before the first frame update
        void Start()
        {
            m_handle.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_useGameplayTime)
            {
                m_handle.Tick(GameplaySystem.time.deltaTime);
            }
            else
            {
                m_handle.Tick(Time.deltaTime);
            }
        }
    }
}