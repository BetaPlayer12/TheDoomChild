
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Playables;

public class CreateCustomTrack : MonoBehaviour
{
    [SerializeField] private AnimationClip animationToAdd;
    [SerializeField] private Animator objectToAnimate;
    [SerializeField] private PlayableDirector director;
    [SerializeField]
    private TimelineAsset m_timeline;
    [SerializeField]
    private string generatedAnimationTrackName;
    [Button]
    public void MakeClip()
    {
        //var track = m_timeline.GetOutputTrack(0);

        //var clip = track.CreateDefaultClip();

        //AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
        TimelineAsset asset = director.playableAsset as TimelineAsset;
      
        foreach (TrackAsset track in asset.GetOutputTracks())
            if (track.name == generatedAnimationTrackName)
                asset.DeleteTrack(track);
        AnimationTrack newTrack = asset.CreateTrack<AnimationTrack>("MyTrackName");

        TimelineClip clip = newTrack.CreateClip(animationToAdd);
        clip.start = 0.5f;
        clip.timeScale = 2f;
        clip.duration = clip.duration / clip.timeScale;

       
        director.SetGenericBinding(newTrack, objectToAnimate);


    }
}
