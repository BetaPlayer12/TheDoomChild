using Cinemachine;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Playables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics.Cutscenes
{
    public class SkillAcquireCutsceneInitializer : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector m_cutscene;
        [SerializeField]
        private PlayableAsset m_cinematic;

        //[SerializeField,BoxGroup("References")]
        //private CinemachineBrain m_cinemachineBrain;
        //[SerializeField, BoxGroup("References")]
        //private CinemachineVirtualCamera m_sceneCamera;
        //[SerializeField, BoxGroup("References")]
        //private SignalEmitter m_signalEmitter;

        //private const string TRACKNAME_CUTSCENE = "SceneCamera";

        private void Awake()
        {
            var timelineAsset = m_cutscene.playableAsset as TimelineAsset;

            var outputTracks = timelineAsset.GetOutputTracks();

            foreach (var track in outputTracks)
            {
                foreach (PlayableBinding binding in track.outputs)
                {
                    switch (binding.sourceObject)
                    {
                        case SpineAnimationStateTrack spineAnimTrack:
                            var playerAnimation = GameplaySystem.playerManager.player.character.GetComponentInChildren<SkeletonAnimation>();
                            m_cutscene.SetGenericBinding(binding.sourceObject, playerAnimation);
                            return;
                        //case CinemachineTrack cinemachineTrack:
                        //    AssignCinemachineTrackData(binding, cinemachineTrack);
                        //    break;
                        //case SignalTrack signalTrack:
                        //    m_cutscene.SetGenericBinding(binding.sourceObject, m_signalEmitter);
                        //    break;
                    }
                }
            }

        }

        //private void AssignCinemachineTrackData(PlayableBinding binding, CinemachineTrack cinemachineTrack)
        //{
        //    m_cutscene.SetGenericBinding(binding.sourceObject, m_cinemachineBrain);
        //    var clipList = cinemachineTrack.GetClips();
        //    foreach (var clip in clipList)
        //    {
        //        if (clip.displayName == "SceneCamera")
        //        {
        //            var shot = (CinemachineShot)clip.asset;
        //            shot.VirtualCamera.defaultValue = m_sceneCamera;
        //        }
        //    }
        //}
    }
}
