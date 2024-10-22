using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Systems;
using DChild.Gameplay.VFX;
using DChild.Menu;
using DChild.Serialization;
using Holysoft.Event;
using System;

namespace DChild.Gameplay
{
    public static class GameplaySystemWrapper
    {
        public static GameplayModifiers modifiers { get => throw new NotImplementedException(); }
        public static AudioListenerPositioner audioListener { get => throw new NotImplementedException(); }
        public static BaseGameplayUIHandle gamplayUIHandle { get => BaseGameplaySystem.gamplayUIHandle; }

        public static ICombatManager combatManager { get => UnderworldGameplaySubsystem.combatManager; }
        public static IFXManager fXManager { get => BaseGameplaySystem.fXManager; }
        public static ICinema cinema { get => BaseGameplaySystem.cinema; }
        public static IWorld world { get => BaseGameplaySystem.world; }
        public static ITime time { get => BaseGameplaySystem.time; }

        public static IPlayerManager playerManager { get => throw new NotImplementedException(); }
        public static ISimulationHandler simulationHandler { get => BaseGameplaySystem.simulationHandler; }
        public static ILootHandler lootHandler { get => UnderworldGameplaySubsystem.lootHandler; }
        public static IHealthTracker healthTracker { get => UnderworldGameplaySubsystem.healthTracker; }
        public static ISoulSkillManager soulSkillManager { get => UnderworldGameplaySubsystem.soulSkillManager; }
        public static IMinionManager minionManager { get => UnderworldGameplaySubsystem.minionManager; }
        public static CampaignSerializer campaignSerializer => BaseGameplaySystem.campaignSerializer;

        public static bool isGamePaused { get; private set; }

        public static void ResumeGame()
        {
            isGamePaused = false;
            BaseGameplaySystem.ResumeGame();
            if(BaseGameplaySystem.GetCurrentWorldType() == WorldType.Underworld)
            {
                UnderworldGameplaySubsystem.ResumeGame();
            }
            else
            {
                OverworldGameplaySubsystem.ResumeGame();
            }
        }

        public static void PauseGame()
        {
            isGamePaused = true;
            BaseGameplaySystem.PauseGame();
            if (BaseGameplaySystem.GetCurrentWorldType() == WorldType.Underworld)
            {
                UnderworldGameplaySubsystem.PauseGame();
            }
            else
            {
                OverworldGameplaySubsystem.PauseGame();
            }
        }

        public static void ClearCaches()
        {
            BaseGameplaySystem.ClearCaches();
            if (BaseGameplaySystem.GetCurrentWorldType() == WorldType.Underworld)
            {
                UnderworldGameplaySubsystem.ClearCaches();
            }
            else
            {
                OverworldGameplaySubsystem.LoadGame();
            }
        }

        public static void LoadGame(CampaignSlot campaignSlot, LoadingHandle.LoadType loadType)
        {
            ClearCaches();
            BaseGameplaySystem.LoadGame(campaignSlot, loadType);
            if (BaseGameplaySystem.GetCurrentWorldType() == WorldType.Underworld)
            {
                UnderworldGameplaySubsystem.LoadGame();
            }
            else
            {
                OverworldGameplaySubsystem.LoadGame();
            }
        }

        public static void ReloadGame()
        {
            BaseGameplaySystem.ReloadGame();
        }

        public static void SetCurrentCampaign(CampaignSlot campaignSlot)
        {
            BaseGameplaySystem.SetCurrentCampaign(campaignSlot);
        }

        public static void SetInputActive(bool isActive)
        {

        }

        public static void ListenToNextSceneLoad()
        {

        }

        private static void LockPlayerToSpawnPosition()
        {

        }

    }
}