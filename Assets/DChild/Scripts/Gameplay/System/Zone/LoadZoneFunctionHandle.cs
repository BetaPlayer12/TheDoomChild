using DChild.Gameplay;
using DChild.Gameplay.Systems.Serialization;
using Sirenix.Serialization.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadZoneFunctionHandle
{
    private LocationData m_locationData;
    private Character m_character;

    public LoadZoneFunctionHandle(LocationData locationData, Character character)
    {
        m_locationData = locationData;
        m_character = character;
    }

    public void CallLocationArriveEvent()
    {
        m_locationData?.CallArriveEvent(m_character);
    }
}
