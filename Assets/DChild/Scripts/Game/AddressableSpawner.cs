using DChild.Gameplay;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DChild
{
    public static class AddressableSpawner
    {
        private struct Request
        {
            public Request(Vector3 position, int index, bool hasCallback)
            {
                this.position = position;
                this.index = index;
                this.hasCallback = hasCallback;
            }

            public Vector3 position { get; }
            public int index { get; }
            public bool hasCallback { get; }
        }

        private static Dictionary<AssetReferenceT<GameObject>, AsyncOperationHandle<GameObject>> m_loadedAssets = new Dictionary<AssetReferenceT<GameObject>, AsyncOperationHandle<GameObject>>();
        private static Dictionary<AssetReferenceT<GameObject>, Cache<List<GameObject>>> m_spawnedInstances = new Dictionary<AssetReferenceT<GameObject>, Cache<List<GameObject>>>();
        private static Dictionary<AssetReferenceT<GameObject>, Cache<Queue<Request>>> m_spawnRequests = new Dictionary<AssetReferenceT<GameObject>, Cache<Queue<Request>>>();
        private static Dictionary<AssetReferenceT<GameObject>, Cache<Queue<Action<GameObject, int>>>> m_spawnRequestsCallback = new Dictionary<AssetReferenceT<GameObject>, Cache<Queue<Action<GameObject, int>>>>();
        private static Dictionary<AssetReferenceT<GameObject>, int> m_toBeInstantiatedCount = new Dictionary<AssetReferenceT<GameObject>, int>();
        public static event Action<GameObject> OnSpawn;

        public static void Spawn(AssetReferenceT<GameObject> asset, Vector3 position, int index = 0, Action<GameObject, int> CallBack = null)
        {
            var request = new Request(position, index, CallBack != null);
            if (m_loadedAssets.ContainsKey(asset))
            {
                if (m_loadedAssets[asset].IsDone)
                {
                    SpawnInstance(asset, request, CallBack);
                }
                else
                {
                    if (m_spawnRequests.ContainsKey(asset) == false)
                    {
                        CreateRequestFor(asset, request, CallBack);
                    }
                    else
                    {
                        m_toBeInstantiatedCount[asset]++;
                        m_spawnRequests[asset].Value.Enqueue(request);
                        if (CallBack != null)
                        {
                            m_spawnRequestsCallback[asset].Value.Enqueue(CallBack);
                        }
                    }
                }
            }
            else
            {
                LoadAndSpawn(asset, request, CallBack);
            }
        }

        private static void LoadAndSpawn(AssetReferenceT<GameObject> asset, Request request, Action<GameObject, int> CallBack)
        {
            var handle = asset.LoadAssetAsync<GameObject>();
            m_loadedAssets.Add(asset, handle);

            var cacheSpawnList = Cache<List<GameObject>>.Claim();
            cacheSpawnList.Value.Clear();
            m_spawnedInstances.Add(asset, cacheSpawnList);

            CreateRequestFor(asset, request, CallBack);

            handle.Completed += (operation) =>
            {
                SpawnFromRequests(asset);
            };
        }

        private static void CreateRequestFor(AssetReferenceT<GameObject> asset, Request request, Action<GameObject, int> CallBack)
        {
            if (m_toBeInstantiatedCount.ContainsKey(asset) == false)
            {
                m_toBeInstantiatedCount.Add(asset, 1);
            }

            if (m_spawnRequests.ContainsKey(asset) == false)
            {
                var cacheRequest = Cache<Queue<Request>>.Claim();
                cacheRequest.Value.Clear();
                m_spawnRequests.Add(asset, cacheRequest);
            }
            m_spawnRequests[asset].Value.Enqueue(request);

            if (CallBack != null)
            {
                if (m_spawnRequestsCallback.ContainsKey(asset) == false)
                {
                    var cacheCallBack = Cache<Queue<Action<GameObject, int>>>.Claim();
                    cacheCallBack.Value.Clear();
                    m_spawnRequestsCallback.Add(asset, cacheCallBack);
                }
                m_spawnRequestsCallback[asset].Value.Enqueue(CallBack);
            }
        }

        private static void SpawnFromRequests(AssetReferenceT<GameObject> asset)
        {
            var cacheRequest = m_spawnRequests[asset];
            do
            {
                var request = cacheRequest.Value.Dequeue();
                SpawnInstance(asset, request, request.hasCallback ? m_spawnRequestsCallback[asset].Value.Dequeue() : null);
            } while (cacheRequest.Value.Count > 0);
            m_spawnRequests[asset].Value.Clear();
            m_spawnRequests[asset].Release();
            m_spawnRequests.Remove(asset);
            if (m_spawnRequestsCallback.ContainsKey(asset))
            {
                m_spawnRequestsCallback[asset].Value.Clear();
                m_spawnRequestsCallback[asset].Release();
                m_spawnRequestsCallback.Remove(asset);
            }
        }

        private static void SpawnInstance(AssetReferenceT<GameObject> asset, Request request, Action<GameObject, int> CallBack)
        {
            m_toBeInstantiatedCount[asset]++;
            asset.InstantiateAsync(request.position, Quaternion.identity).Completed += (operation) =>
            {
                m_spawnedInstances[asset].Value.Add(operation.Result);
                //var instance = operation.Result.AddComponent<AddressableInstance>();
                //instance.reference = asset;
                //instance.OnDestroyInstance = OnInstanceDestroyed;
                m_toBeInstantiatedCount[asset]--;

                if (request.hasCallback)
                {
                    CallBack?.Invoke(operation.Result, request.index);
                }
                OnSpawn?.Invoke(operation.Result);
            };
        }

        private static void OnInstanceDestroyed(AssetReferenceT<GameObject> asset, GameObject instance)
        {
            m_spawnedInstances[asset].Value.Remove(instance);
            if (m_spawnedInstances[asset].Value.Count > 0 && m_toBeInstantiatedCount[asset] == 0)
            {
                m_spawnedInstances[asset].Release();
                m_spawnedInstances.Remove(asset);
                m_toBeInstantiatedCount.Remove(asset);

                Addressables.Release(m_loadedAssets[asset]);
                asset.ReleaseAsset();
                m_loadedAssets.Remove(asset);
            }
        }
    }

}