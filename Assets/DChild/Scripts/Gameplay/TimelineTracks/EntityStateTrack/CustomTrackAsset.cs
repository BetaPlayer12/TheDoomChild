using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CustomTrackAsset :  PlayableAsset
{
   
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CustomTrackBehaviour>.Create(graph);

        var CustomTrackBehaviour = playable.GetBehaviour();
       

        return playable;
    }
}
