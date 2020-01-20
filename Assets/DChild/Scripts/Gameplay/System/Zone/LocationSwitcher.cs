using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [RequireComponent(typeof(LocationPoster))]
    public class LocationSwitch : SerializedMonoBehaviour
    {
        [SerializeField]
        private LocationData m_destination;
        [SerializeField]
        private float m_transitionDelay = 0.5f;
        [SerializeField]
        private ISwitchHandle m_handle;

        private LocationPoster m_poster;

        private void Awake()
        {
            m_poster = GetComponent<LocationPoster>();
            m_poster.data.OnArrival += OnArrival;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var playerControlledObject = collision.GetComponent<Hitbox>();

            if (playerControlledObject != null)
            {
                Character character = collision.GetComponentInParent<Character>();

                if (character != null)
                {
                    GoToDestination(character);
                }
            }
        }

        private IEnumerator DoTransition(Character character, TransitionType type)
        {
            m_handle.DoSceneTransition(character, type);

            if (type == TransitionType.Enter)
            {
                yield return new WaitForSeconds(m_transitionDelay);

                m_handle.DoSceneTransition(character, TransitionType.PostEnter);

                LoadingHandle.SetLoadType(LoadingHandle.LoadType.Smart);
                LoadZoneFunctionHandle loadZoneFunctionHandle = new LoadZoneFunctionHandle(m_destination, character);
                GameSystem.LoadZone(m_destination.scene, true, loadZoneFunctionHandle.CallLocationArriveEvent);
            }
            else if (type == TransitionType.Exit)
            {
                character.transform.position = m_poster.data.position;

                yield return new WaitForSeconds(m_transitionDelay);

                var damageable = character.GetComponent<IDamageable>();
                damageable.SetHitboxActive(true);

                GameplaySystem.playerManager.StopCharacterControlOverride();
            }
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
        }

        private void OnDestroy()
        {
            m_poster.data.OnArrival -= OnArrival;
        }
    }
}
