using DChild.Gameplay.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System;

namespace DChild.Gameplay
{
    public struct FXSpawnHandle<T> where T : FX
    {
        public T InstantiateFX(GameObject fx, Vector3 position, HorizontalDirection direction)
        {
            var instance = InstantiateFX(fx, position);
            instance.SetFacing(direction);
            instance.Play();
            return instance;
        }

        public T InstantiateFX(GameObject fx, Vector3 position, Scene scene)
        {
            var instance = InstantiateFX(fx, position);
            SceneManager.MoveGameObjectToScene(instance.gameObject, scene);
            instance.Play();
            return instance;
        }

        public T InstantiateFX(GameObject fx, Vector3 position)
        {
            var instance = GameplaySystem.fXManager.InstantiateFX<T>(fx, position);
            instance.Play();
            return instance;
        }

        public void InstantiateFX(AssetReferenceFX refence, Action<GameObject, int> Callback)
        {
            GameplaySystem.fXManager.InstantiateFX(refence, Callback);
        }
    }
}