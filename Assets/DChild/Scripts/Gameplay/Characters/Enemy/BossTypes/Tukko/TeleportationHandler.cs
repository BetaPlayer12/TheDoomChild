using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_mask;

    private Vector3 m_castPosition;
    private Vector3 m_destination;

    public Vector3 SetDestination(Vector3 currentPos, Vector3 target, float attackDistanceX, float attackDistanceY)
    {
        var targetPos = target;

        //TeleportPos
        var m_castPos1 = new Vector3(targetPos.x - attackDistanceX, currentPos.y, currentPos.z);//PlayerLeftSide 
        var m_castPos2 = new Vector3(targetPos.x + attackDistanceX, currentPos.y, currentPos.z);//RightSide
        var m_castPos3 = new Vector3(targetPos.x + attackDistanceX, targetPos.y + attackDistanceY, currentPos.z);//UpperRight
        var m_castPos4 = new Vector3(targetPos.x - attackDistanceX, targetPos.y + attackDistanceY, currentPos.z);//UpperLeftSide

        var r = Mathf.Abs(Random.Range(1, 3));
        
        //Needs improvement
        switch (r)
        {
            case 1:
                if (!isHitDetected(m_castPos1, Vector2.left))
                {
                    //Debug.Log("Teleport " + r);
                    return m_castPos1;
                }
                break;
            case 2:
                if (!isHitDetected(m_castPos2, Vector2.right))
                {
                    //Debug.Log("Teleport " + r);
                    return m_castPos2;
                }
                break;
            //case 3:
            //    if (!isHitDetected(m_castPos3, Vector2.up))
            //    {
            //        //Debug.Log("Teleport " + r);
            //        return m_castPos3;
            //    }
            //    break;
            //case 4:
            //    if (!isHitDetected(m_castPos4, Vector2.up))
            //    {
            //        //Debug.Log("Teleport " + r);
            //        return m_castPos4;
            //    }
            //    break;
        }

        return target;
    }

    private bool isHitDetected(Vector3 position, Vector3 direction)
    {
        int hitcount;
        Raycaster.SetLayerMask(m_mask);
        var hit = Raycaster.Cast(position, direction, 1f, false, out hitcount);
        return hitcount > 0;
    }
}