using DChild.Menu;
using Holysoft.Collections;
using UnityEngine;

namespace DChild
{
    public class GameModeValidator : MonoBehaviour
    {
        [SerializeField]
        private SceneInfo m_baseGameplayScene;
        [SerializeField]
        private SceneInfo m_underworldGameplayScene;
        [SerializeField]
        private SceneInfo m_overworldGameplayScene;
        [SerializeField]
        private SceneInfo m_armyBattleGameplayScene;

        private GameMode m_currentGameModeSetup;
        private bool m_baseGameplaySceneActive;
        private bool m_underworldGameplaySceneActive;
        private bool m_overworldGameplaySceneActive;
        private bool m_armyBattleGameplaySceneActive;

        public void SetupGameMode(GameMode gameMode)
        {
            if (m_baseGameplaySceneActive == false)
            {
                LoadingHandle.LoadScenes(m_baseGameplayScene);
                m_baseGameplaySceneActive = true;
            }
            else if (m_currentGameModeSetup != gameMode)
            {
                //Remove Previous Setup
                SetupRemovalOfGameModeSystems(m_currentGameModeSetup);
            }

            m_currentGameModeSetup = gameMode;
            ValidateGameModeSystem(m_currentGameModeSetup);
        }

        private void ValidateGameModeSystem(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Underworld:
                    if (m_underworldGameplaySceneActive == false)
                    {
                        LoadingHandle.LoadScenes(m_underworldGameplayScene);
                        m_underworldGameplaySceneActive = true;
                    }
                    break;
                case GameMode.Overworld:
                    if (m_overworldGameplaySceneActive == false)
                    {
                        LoadingHandle.LoadScenes(m_overworldGameplayScene);
                        m_overworldGameplaySceneActive = true;
                    }
                    break;
                case GameMode.ArmyBattle:
                    if (m_armyBattleGameplaySceneActive == false)
                    {
                        LoadingHandle.LoadScenes(m_armyBattleGameplayScene);
                        m_armyBattleGameplaySceneActive = true;
                    }
                    break;
            }
        }

        private void SetupRemovalOfGameModeSystems(GameMode gameMode)
        {
            switch (m_currentGameModeSetup)
            {
                case GameMode.Underworld:
                    if (m_underworldGameplaySceneActive)
                    {
                        LoadingHandle.UnloadScenes(m_underworldGameplayScene);
                        m_underworldGameplaySceneActive = false;
                    }
                    break;
                case GameMode.Overworld:
                    if (m_overworldGameplaySceneActive)
                    {
                        LoadingHandle.UnloadScenes(m_overworldGameplayScene);
                        m_overworldGameplaySceneActive = false;
                    }
                    break;
                case GameMode.ArmyBattle:
                    if (m_armyBattleGameplaySceneActive)
                    {
                        LoadingHandle.UnloadScenes(m_armyBattleGameplayScene);
                        m_armyBattleGameplaySceneActive = false;
                    }
                    break;
            }
        }
    }

}