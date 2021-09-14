using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CustomTrackAsset :  PlayableAsset
{
    public GameObject m_character;
    public HorizontalDirection m_facing = HorizontalDirection.Right;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CustomTrackBehaviour>.Create(graph);

        var CustomTrackBehaviour = playable.GetBehaviour();
        CustomTrackBehaviour.m_character = m_character;
        CustomTrackBehaviour.m_facing = m_facing;
        return playable;
    }
}
