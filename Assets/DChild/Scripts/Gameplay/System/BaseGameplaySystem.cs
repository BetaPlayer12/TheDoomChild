using DChild.Gameplay;
using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Systems;
using DChild.Gameplay.VFX;
using DChild.Serialization;
using UnityEngine;

public class BaseGameplaySystem : MonoBehaviour
{
    private static BaseGameplaySystem m_instance;
    private static CampaignSlot m_campaignToLoad;

    private static CampaignSerializer m_campaignSerializer;

    public static CampaignSerializer campaignSerializer => m_campaignSerializer;

    #region Modules
    private static IGameplayActivatable[] m_activatableModules;
    private static IOptionalGameplaySystemModule[] m_optionalGameplaySystemModules;
    private static FXManager m_fxManager;
    private static Cinema m_cinema;
    private static World m_world;
    private static SimulationHandler m_simulation;
    private static DChild.Gameplay.Systems.PlayerManager m_playerManager;
    private static ZoneMoverHandle m_zoneMover;
    private static GameplayUIHandle m_gameplayUIHandle;
    
    public static IFXManager fXManager => m_fxManager;
    public static ICinema cinema => m_cinema;
    public static IWorld world => m_world;
    public static ITime time
    {
        get
        {
            if (m_world == null)
            {
                return new TimeInfo(Time.timeScale, Time.deltaTime, Time.fixedDeltaTime);
            }
            else
            {
                return m_world;
            }
        }
    }

    public static IPlayerManager playerManager => m_playerManager;
    public static ISimulationHandler simulationHandler => m_simulation;
    public static IGameplayUIHandle gamplayUIHandle => m_gameplayUIHandle;  
    #endregion

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
