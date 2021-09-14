using DChild.Gameplay.Characters;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CustomTrackBehaviour : PlayableBehaviour
{
    public GameObject m_character = null;
    public HorizontalDirection m_facing = HorizontalDirection.Right;
    public event EventAction<FacingEventArgs> CharacterTurn;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (m_character != null)
        {

           
            if(m_facing == HorizontalDirection.Right)
            {
                m_character.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (m_facing == HorizontalDirection.Left)
            {
                m_character.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
