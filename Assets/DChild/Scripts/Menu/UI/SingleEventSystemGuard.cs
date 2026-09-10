using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DChild.UI
{
    /// <summary>
    /// Keeps additive scenes from competing for UI input through multiple EventSystems.
    /// The bootstrap System scene owns the preferred EventSystem; another loaded scene is
    /// promoted only if that EventSystem is unloaded.
    /// </summary>
    internal static class SingleEventSystemGuard
    {
        private const string PreferredSceneName = "_Scene System";

        private static readonly HashSet<EventSystem> s_disabledEventSystems = new HashSet<EventSystem>();
        private static readonly HashSet<BaseInputModule> s_disabledInputModules = new HashSet<BaseInputModule>();

        private static EventSystem s_primaryEventSystem;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            s_primaryEventSystem = null;
            s_disabledEventSystems.Clear();
            s_disabledInputModules.Clear();

            EnsureSingleEventSystem();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureSingleEventSystem();
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            if (s_primaryEventSystem == null)
            {
                EnsureSingleEventSystem();
            }
        }

        private static void EnsureSingleEventSystem()
        {
            EventSystem[] eventSystems = Object.FindObjectsOfType<EventSystem>(true);
            if (eventSystems.Length == 0)
            {
                s_primaryEventSystem = null;
                return;
            }

            EventSystem preferredEventSystem = FindPreferred(eventSystems);
            if (preferredEventSystem != null && preferredEventSystem != s_primaryEventSystem)
            {
                s_primaryEventSystem = preferredEventSystem;
                RestorePrimary(s_primaryEventSystem);
            }
            else if (!IsUsable(s_primaryEventSystem))
            {
                s_primaryEventSystem = SelectPrimary(eventSystems);
                RestorePrimary(s_primaryEventSystem);
            }

            if (s_primaryEventSystem == null)
            {
                return;
            }

            EventSystem.current = s_primaryEventSystem;

            for (int i = 0; i < eventSystems.Length; i++)
            {
                EventSystem eventSystem = eventSystems[i];
                if (eventSystem == null || eventSystem == s_primaryEventSystem || !eventSystem.gameObject.activeInHierarchy)
                {
                    continue;
                }

                DisableDuplicate(eventSystem);
            }
        }

        private static EventSystem FindPreferred(IReadOnlyList<EventSystem> eventSystems)
        {
            for (int i = 0; i < eventSystems.Count; i++)
            {
                EventSystem candidate = eventSystems[i];
                if (candidate == null
                    || !candidate.gameObject.activeInHierarchy
                    || !candidate.gameObject.scene.isLoaded
                    || candidate.gameObject.scene.name != PreferredSceneName)
                {
                    continue;
                }

                if (candidate.isActiveAndEnabled || s_disabledEventSystems.Contains(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static EventSystem SelectPrimary(IReadOnlyList<EventSystem> eventSystems)
        {
            EventSystem current = EventSystem.current;
            EventSystem firstEnabled = null;
            EventSystem firstGuardDisabled = null;

            for (int i = 0; i < eventSystems.Count; i++)
            {
                EventSystem candidate = eventSystems[i];
                if (candidate == null || !candidate.gameObject.activeInHierarchy || !candidate.gameObject.scene.isLoaded)
                {
                    continue;
                }

                if (candidate == current && candidate.isActiveAndEnabled)
                {
                    firstEnabled = candidate;
                }
                else if (firstEnabled == null && candidate.isActiveAndEnabled)
                {
                    firstEnabled = candidate;
                }

                if (firstGuardDisabled == null && s_disabledEventSystems.Contains(candidate))
                {
                    firstGuardDisabled = candidate;
                }
            }

            return firstEnabled != null ? firstEnabled : firstGuardDisabled;
        }

        private static bool IsUsable(EventSystem eventSystem)
        {
            return eventSystem != null
                && eventSystem.isActiveAndEnabled
                && eventSystem.gameObject.scene.isLoaded;
        }

        private static void RestorePrimary(EventSystem eventSystem)
        {
            if (eventSystem == null)
            {
                return;
            }

            if (s_disabledEventSystems.Remove(eventSystem))
            {
                eventSystem.enabled = true;
            }

            BaseInputModule[] inputModules = eventSystem.GetComponents<BaseInputModule>();
            for (int i = 0; i < inputModules.Length; i++)
            {
                BaseInputModule inputModule = inputModules[i];
                if (inputModule != null && s_disabledInputModules.Remove(inputModule))
                {
                    inputModule.enabled = true;
                }
            }
        }

        private static void DisableDuplicate(EventSystem eventSystem)
        {
            bool disabledSomething = false;
            BaseInputModule[] inputModules = eventSystem.GetComponents<BaseInputModule>();
            for (int i = 0; i < inputModules.Length; i++)
            {
                BaseInputModule inputModule = inputModules[i];
                if (inputModule != null && inputModule.enabled)
                {
                    inputModule.enabled = false;
                    s_disabledInputModules.Add(inputModule);
                    disabledSomething = true;
                }
            }

            if (eventSystem.enabled)
            {
                eventSystem.enabled = false;
                s_disabledEventSystems.Add(eventSystem);
                disabledSomething = true;
            }

            if (!disabledSomething)
            {
                return;
            }

            Debug.Log(
                $"Disabled duplicate EventSystem '{eventSystem.name}' from scene " +
                $"'{eventSystem.gameObject.scene.name}'. Primary EventSystem is in " +
                $"'{s_primaryEventSystem.gameObject.scene.name}'.");
        }
    }
}
