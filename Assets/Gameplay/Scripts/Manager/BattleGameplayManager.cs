using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Inspector;

namespace DragonCrashers
{
    public enum CutsceneMode
    {
        Play,
        None
    }

    public class BattleGameplayManager : MonoBehaviour
    {

        [Header("Teams")]
        public List<UnitController> heroTeamUnits;
        public List<UnitController> enemyTeamUnits;

        [Header("Team Logic")]
        public bool autoAssignUnitTeamTargets = false;

        //Runtime Battle Logic
        private List<UnitController> aliveHeroUnits;
        private List<UnitController> aliveEnemyUnits;

        [Header("Battle Intro")]
        public CutsceneMode introCutscene;
        public CutsceneTimelineBehaviour introCutsceneBehaviour;

        [Header("Battle Ended - Victory")]
        public CutsceneTimelineBehaviour victoryCutsceneBehaviour;
        public SceneField victoryNextScene;

        [Header("Battle Ended - Defeat")]
        public CutsceneTimelineBehaviour defeatCutsceneBehaviour;
        public SceneField defeatNextScene;

        private SceneField selectedNextScene;

        [Header("Screen Fader")]
        public ScreenFaderManager screenFaderManager;
        

        void Awake()
        {  
            
            SetupTeamUnits();
            StartGameLogic();
        }

        void SetupTeamUnits()
        {
            CreateAliveUnits();

            if(autoAssignUnitTeamTargets)
            {
                AutoAssignUnitTeamTargets();
            }

        }

        void CreateAliveUnits()
        {
            aliveHeroUnits = new List<UnitController>();

            for(int i = 0; i < heroTeamUnits.Count; i++)
            {
                aliveHeroUnits.Add(heroTeamUnits[i]);
                aliveHeroUnits[i].SetAlive();
                aliveHeroUnits[i].UnitDiedEvent += UnitHasDied;
            }

            aliveEnemyUnits = new List<UnitController>();

            for(int i = 0; i < enemyTeamUnits.Count; i++)
            {
                aliveEnemyUnits.Add(enemyTeamUnits[i]);
                aliveEnemyUnits[i].SetAlive();
                aliveEnemyUnits[i].UnitDiedEvent += UnitHasDied;
            } 
        }



        void AutoAssignUnitTeamTargets()
        {
            for(int i = 0; i < aliveHeroUnits.Count; i++)
            {
                aliveHeroUnits[i].AssignTargetUnits(aliveEnemyUnits);
            }

            for(int i = 0; i < aliveEnemyUnits.Count; i++)
            {
                aliveEnemyUnits[i].AssignTargetUnits(aliveHeroUnits);
            }
        }

        void StartGameLogic()
        {
            switch(introCutscene)
            {
                case CutsceneMode.Play:
                    StartIntroCutscene();
                    break;

                case CutsceneMode.None:
                    StartBattle();
                    break;
            }
        }

        void StartIntroCutscene()
        {
            introCutsceneBehaviour.StartTimeline();
        }

        public void StartBattle()
        {
            for(int i = 0; i < aliveHeroUnits.Count; i++)
            {
                aliveHeroUnits[i].BattleStarted();
            }

            for(int i = 0; i < aliveEnemyUnits.Count; i++)
            {
                aliveEnemyUnits[i].BattleStarted();
            }
        }

        void UnitHasDied(UnitController deadUnit)
        {
            RemoveUnitFromAliveUnits(deadUnit);

        }

        void RemoveUnitFromAliveUnits(UnitController unit)
        {
            CheckRemainingTeams();
            for(int i = 0; i < aliveHeroUnits.Count; i++)
            {
                if(unit == aliveHeroUnits[i])
                {
                    aliveHeroUnits[i].UnitDiedEvent -= UnitHasDied;
                    aliveHeroUnits.RemoveAt(i); 
                    RemoveUnitFromEnemyTeamTargets(unit);
                }
            }

            for(int i = 0; i < aliveEnemyUnits.Count; i++)
            {
                if(unit == aliveEnemyUnits[i])
                {
                    aliveEnemyUnits[i].UnitDiedEvent -= UnitHasDied;
                    aliveEnemyUnits.RemoveAt(i);
                    RemoveUnitFromHeroTeamTargets(unit);
                }
            }

            CheckRemainingTeams();
        }

        void RemoveUnitFromHeroTeamTargets(UnitController unit)
        {
            for(int i = 0; i < aliveHeroUnits.Count; i++)
            {
                aliveHeroUnits[i].RemoveTargetUnit(unit);
            }
        }

        void RemoveUnitFromEnemyTeamTargets(UnitController unit)
        {
            for(int i = 0; i < aliveEnemyUnits.Count; i++)
            {
                aliveEnemyUnits[i].RemoveTargetUnit(unit);
            }
        }

        void CheckRemainingTeams()
        {

            if(aliveHeroUnits.Count == 0)
            {
                SetBattleDefeat();
            }

            if(aliveEnemyUnits.Count == 0)
            {
                SetBattleVictory();
            }
        }

        void SetBattleVictory()
        {
            StopAllAliveTeamUnits(aliveHeroUnits);

            if(victoryCutsceneBehaviour != null)
            {
                victoryCutsceneBehaviour.StartTimeline();
            }
        }

        void SetBattleDefeat()
        {
            StopAllAliveTeamUnits(aliveEnemyUnits);
            
            if(defeatCutsceneBehaviour != null)
            {
                defeatCutsceneBehaviour.StartTimeline();
            }          
        }

        void StopAllAliveTeamUnits(List<UnitController> aliveTeamUnits)
        {
            for(int i = 0; i < aliveTeamUnits.Count; i++)
            {
                aliveTeamUnits[i].BattleEnded();
            }
        }
        
        public void SelectVictoryNextScene()
        {
            selectedNextScene = victoryNextScene;
            screenFaderManager.StartFadeToBlack();
        }

        public void SelectDefeatNextScene()
        {
            selectedNextScene = defeatNextScene;
            screenFaderManager.StartFadeToBlack();
        }

        public void LoadSelectedScene()
        {
            NextSceneLoader sceneLoader = new NextSceneLoader();
            sceneLoader.LoadNextScene(selectedNextScene);
        }
        

        



    }

}