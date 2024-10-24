using DarkTonic.MasterAudio;
using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Serialization;
using DChild.Gameplay.VFX;
using DChild.Menu;
using DChild.Serialization;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class BaseGameplaySystem : MonoBehaviour
{
    private static BaseGameplaySystem m_instance;
    private static CampaignSlot m_campaignToLoad;
    private static GameplayModifiers m_modifiers;
    public static GameplayModifiers modifiers => m_modifiers;

    private static CampaignSerializer m_campaignSerializer;

    public static CampaignSerializer campaignSerializer => m_campaignSerializer;

    public static AudioListenerPositioner audioListener { get; private set; }

    [SerializeField]
    private static WorldTypeManager m_worldTypeManager;

    #region Modules
    private static IGameplayActivatable[] m_activatableModules;
    private static IOptionalGameplaySystemModule[] m_optionalGameplaySystemModules;
    private static FXManager m_fxManager;
    private static Cinema m_cinema;
    private static World m_world;
    private static SimulationHandler m_simulation;

    private static ZoneMoverHandle m_zoneMover;
    private static BaseGameplayUIHandle m_baseGameplayUIHandle;
    public static bool isGamePaused { get; private set; }

    public static BaseGameplayUIHandle gamplayUIHandle => m_baseGameplayUIHandle;
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
    
    public static ISimulationHandler simulationHandler => m_simulation;
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

    public static WorldType GetCurrentWorldType()
    {
        return m_worldTypeManager.CurrentWorldType;
    }

    private void Start()
    {
        if (m_campaignToLoad != null)
        {
            m_campaignSerializer.SetSlot(m_campaignToLoad);

            m_campaignToLoad = null;
        }
    }

    public static void ResumeGame()
    {
        GameTime.UnregisterValueChange(m_instance, GameTime.Factor.Multiplication);
        isGamePaused = false;
        GameSystem.SetCursorVisibility(false);

        try
        {
            MasterAudio.UnpauseEverything();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void PauseGame()
    {
        GameTime.RegisterValueChange(m_instance, 0, GameTime.Factor.Multiplication);      
        isGamePaused = true;
        GameSystem.SetCursorVisibility(true);
        MasterAudio.PauseEverything();
        SkeletonAnimationManager.Instance.PauseAllSpines();
    }

    public static void ClearCaches()
    {
        MasterAudio.StopMixer();
        m_cinema?.ClearLists();
    }

    public static void LoadGame(CampaignSlot campaignSlot, LoadingHandle.LoadType loadType)
    {
        m_campaignToLoad = campaignSlot;
        ClearCaches();
        PersistentDataManager.ApplySaveData(campaignSlot.dialogueSaveData, DatabaseResetOptions.KeepAllLoaded);
        LoadingHandle.SetLoadType(loadType);
        if(GameSystem.m_useGameModeValidator)
        {
            var WorldTypeVar = FindObjectOfType<WorldTypeManager>();

            WorldTypeVar.SetCurrentWorldType(m_campaignToLoad.location);

            switch (WorldTypeVar.CurrentWorldType)
            {
                case WorldType.Underworld:
                    GameSystem.LoadZone(GameMode.Underworld, m_campaignToLoad.sceneToLoad, true);
                    break;
                case WorldType.Overworld:
                    GameSystem.LoadZone(GameMode.Overworld, m_campaignToLoad.sceneToLoad, true);
                    break;
                case WorldType.ArmyBattle:
                    GameSystem.LoadZone(GameMode.ArmyBattle, m_campaignToLoad.sceneToLoad, true);
                    break;
            }
        }
        else
        {
            GameSystem.LoadZone(m_campaignToLoad.sceneToLoad, true);
        }
        //Reload Items
        LoadingHandle.SceneDone += LoadGameDone;
    }

    private static void LoadGameDone(object sender, EventActionArgs eventArgs)
    {
        LoadingHandle.SceneDone -= LoadGameDone;


        m_campaignSerializer.SetSlot(m_campaignToLoad);
        m_baseGameplayUIHandle.ResetGameplayUI();
        m_campaignSerializer.Load(SerializationScope.Gameplay, true);
    }

    public static void ReloadGame()
    {
        LoadGame(campaignSerializer.slot, LoadingHandle.LoadType.Force);
    }

    public static void SetCurrentCampaign(CampaignSlot campaignSlot)
    {
        if (m_instance)
        {
            LoadGame(campaignSlot, LoadingHandle.LoadType.Force);
        }
        else
        {
            m_campaignToLoad = campaignSlot;
        }
    }
}
