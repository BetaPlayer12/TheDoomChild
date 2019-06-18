using DChild;
using DChild.Configurations;
using DChild.Configurations.Visuals;
using DChild.Gameplay;
using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Databases;
using DChild.Gameplay.Databases.Components;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems;
using DChild.Gameplay.VFX;
using DChild.Menu;
using UnityEditor;
using UnityEngine;
using AudioSettings = DChild.Configurations.AudioSettings;

namespace DChildEditor.Toolkit
{
    public class SystemSetup
    {
        [MenuItem("Tools/Kit/Setup Systems")]
        private static void SetupSystem()
        {
            var systemGO = Commands.CreateGameObject("System", null, Vector3.zero);
            systemGO.AddComponent<GameSystem>();
            CreateGameSettings(systemGO);
            CreateGameplaySystem(systemGO);
        }

        private static void CreateGameplaySystem(GameObject systemGO)
        {
            var gameplaySystem = Commands.CreateGameObject("GameplaySystem", systemGO.transform, Vector3.zero);
            gameplaySystem.AddComponent<GameplaySystem>();
            gameplaySystem.AddComponent<PoolManager>();
            gameplaySystem.AddComponent<FXManager>();
            gameplaySystem.AddComponent<World>();
            gameplaySystem.AddComponent<AstarPath>();
            var cinema = gameplaySystem.AddComponent<Cinema>();
            cinema.Initialized(Camera.main);

            var databaseManager = Commands.CreateGameObject("DatabaseManager", gameplaySystem.transform, Vector3.zero);
            databaseManager.AddComponent<DatabaseManager>();
            var damageComponent = databaseManager.AddComponent<DamageUIDataComponent>();
            damageComponent.Initialize(DChildResources.LoadScriptableObject<DamageUIConfigurations>("DamageUIConfigurations"));

            var combatManager = Commands.CreateGameObject("CombatManager", gameplaySystem.transform, Vector3.zero);
            combatManager.AddComponent<CombatManager>();
            var playerCombatHandler = combatManager.AddComponent<PlayerCombatHandler>();
            playerCombatHandler.Initialize(50f, 1f, 1f);
        }

        private static void CreateGameSettings(GameObject systemGO)
        {
            var gameSettingsGO = Commands.CreateGameObject("GameSettings", systemGO.transform, Vector3.zero);
            var gameSettings = gameSettingsGO.AddComponent<GameSettings>();
            gameSettings.Initialize(DChildResources.LoadScriptableObject<SupportedResolutions>("SupportedResolutions"));

            var visualConfigurations = Commands.CreateGameObject("VisualConfigurations", gameSettingsGO.transform, Vector3.zero);
            var screenResolution = visualConfigurations.AddComponent<ScreenResolution>();
            var screenLighting = visualConfigurations.AddComponent<ScreenLighting>();
        }
    }
}