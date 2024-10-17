using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [RequireComponent(typeof(LocationPoster))]
    public class LocationSwitcher : SerializedMonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private LocationData m_destination;

        [SerializeField]
        private ISwitchHandle m_handle;

        private LocationPoster m_poster;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => m_handle.needsButtonInteraction;

        public Vector3 promptPosition => m_handle.promptPosition;

        public string promptMessage => m_handle.prompMessage;

        // for testing
        public LocationData locationData => m_destination;

        public void Interact(Character character)
        {
            if (m_handle.isDebugSwitchHandle)
            {
                m_handle.DoSceneTransition(character, TransitionType.Enter);
            }
            else
            {
                var controller = GameplaySystem.playerManager.OverrideCharacterControls();
                StartCoroutine(DoTransition(character, TransitionType.Enter));
            }
        }

        [Button]
        public void ForceActivation()
        {
            if (m_handle.needsButtonInteraction)
            {
                Interact(GameplaySystem.playerManager.player.character);
            }
            else
            {
                GoToDestination(GameplaySystem.playerManager.player.character);
            }
        }

        private IEnumerator DoTransition(Character character, TransitionType type)
        {
            m_handle.DoSceneTransition(character, type);

            var WorldTypeVar = FindObjectOfType<WorldTypeManager>();

            if (type == TransitionType.Enter)
            {
                GameplaySystem.playerManager.ReturnPlayerToOrginalScene();
                GameplaySystem.campaignSerializer.UpdateData(SerializationScope.Zone);

                yield return new WaitForSeconds(m_handle.transitionDelay);

                m_handle.DoSceneTransition(character, TransitionType.PostEnter);

                LoadingHandle.SetLoadType(LoadingHandle.LoadType.Smart);
                Cache<LoadZoneFunctionHandle> cacheLoadZoneHandle = Cache<LoadZoneFunctionHandle>.Claim();
                cacheLoadZoneHandle.Value.Initialize(m_destination, character, cacheLoadZoneHandle);

                //Remove when subsystem implementation is complete
                if (GameSystem.m_useGameModeValidator)
                {
                    WorldTypeVar.SetCurrentWorldType(m_destination.location);

                    if (WorldTypeVar.CurrentWorldType == WorldType.Underworld)
                    {
                        GameSystem.LoadZone(GameMode.Underworld, m_destination.sceneInfo, true, cacheLoadZoneHandle.Value.CallLocationArriveEvent);
                    }
                    else
                    {
                        GameSystem.LoadZone(GameMode.Overworld, m_destination.sceneInfo, true, cacheLoadZoneHandle.Value.CallLocationArriveEvent);
                    }
                }
                else
                {
                    GameSystem.LoadZone(m_destination.sceneInfo, true, cacheLoadZoneHandle.Value.CallLocationArriveEvent);
                }
                
                GameplaySystem.ClearCaches();

            }
            else if (type == TransitionType.Exit)
            {
                //character.transform.position = m_poster.data.position;
                LoadingHandle.LoadingDone += OnLoadingDone;

                yield return new WaitForSeconds(m_handle.transitionDelay);

                m_handle.DoSceneTransition(character, TransitionType.PostExit);

                var damageable = character.GetComponent<IDamageable>();
                damageable.SetHitboxActive(true);
                character.GetComponent<Rigidbody2D>().WakeUp();
            }
        }

        private void OnLoadingDone(object sender, EventActionArgs eventArgs)
        {
            GameplaySystem.playerManager.StopCharacterControlOverride();
            LoadingHandle.LoadingDone -= OnLoadingDone;
        }

        public void GoToDestination(Character character)
        {
            var damageable = character.GetComponent<IDamageable>();
            damageable?.SetHitboxActive(false);

            var controller = GameplaySystem.playerManager.OverrideCharacterControls();

            StartCoroutine(DoTransition(character, TransitionType.Enter));
        }

        public void OnArrival(object sender, CharacterEventArgs eventArgs)
        {
            StartCoroutine(DoTransition(eventArgs.character, TransitionType.Exit));
            Debug.LogError("Exit");
        }

        private void Awake()
        {
            m_poster = GetComponent<LocationPoster>();
            m_poster.data.OnArrival += OnArrival;
            Debug.Log($"{m_poster.name} is Logged", this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_handle.needsButtonInteraction == false)
            {
                if (collision.TryGetComponent(out Hitbox hitbox))
                {
                    Character character = collision.GetComponentInParent<Character>();

                    if (character != null)
                    {
                        GoToDestination(character);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            m_poster.data.OnArrival -= OnArrival;
        }

        private void OnDrawGizmosSelected()
        {
            if (showPrompt)
            {
                var position = promptPosition;
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(position, 1f);
            }
        }
    }
}
