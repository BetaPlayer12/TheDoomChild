using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateSlope : MonoBehaviour
{
    private CharacterPhysics2D m_characterPhysics;

    private void Start()
    {
        m_characterPhysics = GetComponentInParent<CharacterPhysics2D>();
    }

    public float CalculateGravity(float walkableAngle, float groundAngle, float defaultGravity)
    {
        var anglePercentage = groundAngle / walkableAngle;
        var modifiedGravity = defaultGravity - (defaultGravity * anglePercentage);
        return modifiedGravity;
    }
}
