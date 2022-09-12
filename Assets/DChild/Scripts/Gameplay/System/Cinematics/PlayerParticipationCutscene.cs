/************************************
 * 
 * A Cutscene is Played when player
 * is inside the trigger
 * 
 ************************************/

using DChild.Gameplay.Characters.Players;
using Spine.Unity;
using Spine.Unity.Playables;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    public class PlayerParticipationCutscene : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector m_cutscene;
        [SerializeField]
        private bool m_stopPlayerControlOverrideAfterCutscene = true;

        private PlayerControlledObject m_controlledObject;
        private Animator m_animator;
        private Scene m_originalScene;

        public void RemovePlayerFromCutscene()
        {
            m_controlledObject.transform.parent = null;
            SceneManager.MoveGameObjectToScene(m_controlledObject.gameObject, m_originalScene);
            m_animator.enabled = true;
            if (m_stopPlayerControlOverrideAfterCutscene)
            {
                GameplaySystem.playerManager.StopCharacterControlOverride();
            }
        }

        private void OnCutscenePlay(PlayableDirector obj)
        {
            m_controlledObject = GameplaySystem.playerManager.player.character.GetComponent<PlayerControlledObject>();
            m_originalScene = m_controlledObject.gameObject.scene;
            m_animator = m_controlledObject.GetComponentInChildren<Animator>();
            m_controlledObject.transform.parent = m_cutscene.transform;
            var rigidBody = m_controlledObject.GetComponent<Rigidbody2D>();
            var velocity = rigidBody.velocity;
            velocity.x = 0;
            GameplaySystem.playerManager.OverrideCharacterControls();
            rigidBody.velocity = velocity;
            m_animator = m_controlledObject.GetComponentInChildren<Animator>();
        }

        private void OnCutsceneDone(PlayableDirector obj)
        {
            RemovePlayerFromCutscene();
        }

        private void Awake()
        {
            m_cutscene.played += OnCutscenePlay;
            m_cutscene.stopped += OnCutsceneDone;


            var animation = GameplaySystem.playerManager.player.character.GetComponentInChildren<SkeletonAnimation>();
            var playerObject = GameplaySystem.playerManager.player.character.gameObject;
            var timelineAsset = m_cutscene.playableAsset as TimelineAsset;


            for (int i = 0; i < timelineAsset.rootTrackCount; i++)
            {
                var rootTrack = timelineAsset.GetRootTrack(i);

                if (rootTrack.name == "Player")
                {
                    var childtracks = rootTrack.GetChildTracks();

                    foreach (var track in childtracks)
                    {
                        var output = track.outputs;
                        foreach (PlayableBinding binding in output)
                        {

                            if (binding.sourceObject is SpineAnimationStateTrack)
                            {
                                m_cutscene.SetGenericBinding(binding.sourceObject, animation);
                            }

                            if (binding.sourceObject is ActivationTrack)
                            {
                                m_cutscene.SetGenericBinding(binding.sourceObject, playerObject);
                            }
                        }
                    }
                    break;
                }


            }


        }
    }
}