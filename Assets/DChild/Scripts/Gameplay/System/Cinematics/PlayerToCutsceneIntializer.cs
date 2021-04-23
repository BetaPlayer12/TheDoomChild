/************************************
 * 
 * A Cutscene is Played when player
 * is inside the trigger
 * 
 ************************************/

using Spine.Unity;
using Spine.Unity.Playables;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    public class PlayerToCutsceneIntializer : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector m_playable;

        private void Awake()
        {
            var animation = GameplaySystem.playerManager.player.character.GetComponentInChildren<SkeletonAnimation>();
            var timelineAsset = m_playable.playableAsset as TimelineAsset;
            var trackCount = timelineAsset.rootTrackCount;
            for (int i = 0; i < trackCount; i++)
            {
                var root = timelineAsset.GetRootTrack(i);
                if (root.name == "Player")
                {
                    foreach (PlayableBinding binding in root.outputs)
                    {
                        if (binding.sourceObject is SpineAnimationStateTrack)
                        {
                            var track = (TrackAsset)binding.sourceObject;
                            m_playable.SetGenericBinding(binding.sourceObject, animation);
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }
}