﻿using DChild.Gameplay;
using DChild.Gameplay.Systems.Serialization;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadZoneFunctionHandle
{
    private LocationData m_locationData;
    private Character m_character;

    private Cache<LoadZoneFunctionHandle> m_cacheVersion;

    public LoadZoneFunctionHandle()
    {
        m_locationData = null;
        m_character = null;
    }

    public void Initialize(LocationData locationData, Character character, Cache<LoadZoneFunctionHandle> cacheVersion)
    {
        m_locationData = locationData;
        m_character = character;
        m_cacheVersion = cacheVersion;
    }

    public void CallLocationArriveEvent()
    {
        m_locationData?.CallArriveEvent(m_character);
        m_cacheVersion.Release();
        m_cacheVersion = null;
    }
}
