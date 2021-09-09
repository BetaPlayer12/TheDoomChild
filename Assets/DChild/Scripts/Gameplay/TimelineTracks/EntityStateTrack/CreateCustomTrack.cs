
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine;
using Sirenix.OdinInspector;

public class CreateCustomTrack : MonoBehaviour
{
    
    [SerializeField]
    private TimelineAsset m_timeline;
    [Button]
    public void MakeClip()
    {
        var track = m_timeline.GetOutputTrack(0);

        var clip = track.CreateDefaultClip();

        AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;

       
    }
}
