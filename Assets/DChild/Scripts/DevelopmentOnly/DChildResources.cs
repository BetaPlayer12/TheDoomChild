#if UNITY_EDITOR
using Spine.Unity;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DChildEditor
{
    public static class DChildResources
    {
        public static string ResourceFolder => "Assets/DChildNew/Objects";
        public static string SpineFolder => $"{ResourceFolder}/Spines";
        public static string ScriptableObjectFolder => $"{ResourceFolder}/ScriptableObjects";
        public static string PrefabsFolder => $"{ResourceFolder}/Prefabs";
        public static string ShadersFolder => $"{ResourceFolder}/Shaders";
        public static string PhysicsMaterial2DFolder => $"{ResourceFolder}/Shaders";

        public static SkeletonDataAsset LoadSpine(string path, string fileName)
        {
            return AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>($"{SpineFolder}/{path}/{fileName}.asset");
        }

        public static GameObject LoadPrefab(string path, string fileName)
        {
            return (GameObject)AssetDatabase.LoadAssetAtPath<Object>($"{PrefabsFolder}/{path}/{fileName}.prefab");
        }

        public static T LoadScriptableObject<T>(string fileName) where T : ScriptableObject
        {
            return AssetDatabase.LoadAssetAtPath<T>($"{ScriptableObjectFolder}/{fileName}.asset");
        }

        public static Shader LoadShader(string fileName)
        {
            return AssetDatabase.LoadAssetAtPath<Shader>($"{ShadersFolder}/{fileName}.shader");
        }

        public static PhysicsMaterial2D LoadPhysicsMaterial2D(string fileName)
        {
            return AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>($"{PhysicsMaterial2DFolder}/{fileName}.physicsMaterial2D");
        }
    }
}
#endif