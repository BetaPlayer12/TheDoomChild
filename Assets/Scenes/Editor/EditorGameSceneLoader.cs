using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorGameSceneLoader : Editor
{

    static string scenesFolderLocation = "Assets/Scenes/";
    
    static string scenePrefabLineup = "Prefabs/Scene_Prefabs_Lineup.unity";

    static string sceneGameMenu = "Gameplay/Scene_Game_Menu.unity";
    static string sceneBattleSkeletons = "Gameplay/Scene_Battle_Skeletons.unity";
    static string sceneBattleDragon = "Gameplay/Scene_Battle_Dragon.unity";

    
    [MenuItem("Dragon Crashers/Scenes/Load Prefab Lineup")]
    public static void LoadPrefabLineupScene()
    {
        LoadScene(scenePrefabLineup);
    }

    [MenuItem("Dragon Crashers/Scenes/Load Game Menu")]
    public static void LoadGameMenuScene()
    {
        LoadScene(sceneGameMenu);
    }

    [MenuItem("Dragon Crashers/Scenes/Load Skeleton Battle")]
    public static void LoadSkeletonBattleScene()
    {
        LoadScene(sceneBattleSkeletons);
    }

    [MenuItem("Dragon Crashers/Scenes/Load Dragon Battle")]
    public static void LoadDragonBattleScene()
    {
        LoadScene(sceneBattleDragon);
    }
    

    
    static void LoadScene(string selectedScenePath)
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenesFolderLocation + selectedScenePath, OpenSceneMode.Single);
        }
    }
    

    /*
    static void LoadSceneSet(int sceneSet)
    {
        if(sceneSet == 0)
        {
            var mainScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Scene_Dev_Gameplay.unity", UnityEditor.SceneManagement.OpenSceneMode.Single);
            var subScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Scene_Dev_Environment.unity", UnityEditor.SceneManagement.OpenSceneMode.Additive);
        }

        if(sceneSet == 1)
        {
            var mainScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Scene_Dev_AssetLineup.unity", UnityEditor.SceneManagement.OpenSceneMode.Single);

        }

    }
    */


}