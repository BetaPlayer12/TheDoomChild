using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.AI;

public class CharacterStickToWall : MonoBehaviour
{
    //THIS SCRIPT IS TEMPORARY
    [SerializeField]
    private Transform m_model;
    [SerializeField]
    private RaySensor m_edgeWallSensor;

    private PlatformPatrolSensor m_platformSensor;
    private ShiftingCharacterPhysics2D m_shiftPhysics;
    private WorldOrientation m_orientation;

    private void GetOrientation()
    {
        if (!m_edgeWallSensor.isDetecting)
        {
            if (m_model.localRotation.y == 0)
            {
                m_model.localRotation = Quaternion.Euler(0, 0, 90);
                if(transform.localScale.x == 1)
                {
                    m_shiftPhysics.SetOrientation(WorldOrientation.Right);
                }
                else
                {
                    m_shiftPhysics.SetOrientation(WorldOrientation.Left);
                }
            }
        }
    }

    private void Awake()
    {
        m_shiftPhysics = GetComponent<ShiftingCharacterPhysics2D>();
    }

    private void Update()
    {
        GetOrientation();
    }
}
