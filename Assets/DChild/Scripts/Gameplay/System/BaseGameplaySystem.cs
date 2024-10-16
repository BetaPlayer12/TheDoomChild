using DChild.Gameplay;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using UnityEngine;

public class BaseGameplaySystem : MonoBehaviour
{
    private static BaseGameplaySystem m_instance;
    private static CampaignSlot m_campaignToLoad;

    private static CampaignSerializer m_campaignSerializer;

    public static CampaignSerializer campaignSerializer => m_campaignSerializer;


    private void AssignModules()
    {
        AssignModule(out m_campaignSerializer);
    }

    private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();


    protected void Awake()
    {
        if (m_instance)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
            AssignModules();
            var initializables = GetComponentsInChildren<IGameplayInitializable>();
            for (int i = 0; i < initializables.Length; i++)
            {
                initializables[i].Initialize();
            }
            if (m_campaignToLoad != null)
            {
                m_campaignSerializer.SetSlot(m_campaignToLoad);
            }
        }
    }

    private void Start()
    {
        if (m_campaignToLoad != null)
        {
            m_campaignSerializer.SetSlot(m_campaignToLoad);

            m_campaignToLoad = null;
        }
    }
}
