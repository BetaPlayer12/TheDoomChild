using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay;
using UnityEngine;

[RequireComponent(typeof(Portal))]
public class ShiftingPortal : MonoBehaviour
{
    public WorldOrientation m_shiftTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hitbox")
        {
            var dummy = collision.GetComponentInParent<IWorldShifter>();
            if (dummy != null)
            {
                dummy.SetOrientation(m_shiftTo);
            }
        }
    }
}
