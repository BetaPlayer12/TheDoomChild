using DChild;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaofEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject m_toCreate;

    protected Vector2 RoofPosition(Vector2 startPoint)
    {
        int hitCount = 0;
        //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
        RaycastHit2D[] hit = Cast(startPoint, Vector2.up, 100f, true, out hitCount, true);
        if (hit != null)
        {
            Debug.DrawRay(startPoint, hit[0].point);
            return hit[0].point;
        }
        return Vector2.zero;
        //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
        //return hitPos;
    }
    protected Vector2 FloorPosition(Vector2 startPoint)
    {
        int hitCount = 0;
        //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
        RaycastHit2D[] hit = Cast(startPoint, Vector2.down, 100f, true, out hitCount, true);
        if (hit != null)
        {
            Debug.DrawRay(startPoint, hit[0].point);
            return hit[0].point;
        }
        return Vector2.zero;
        //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
        //return hitPos;
    }
    protected Vector2 WallRightPosition(Vector2 startPoint)
    {
        int hitCount = 0;
        //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
        RaycastHit2D[] hit = Cast(startPoint, Vector2.right, 100f, true, out hitCount, true);
        if (hit != null)
        {
            Debug.DrawRay(startPoint, hit[0].point);
            return hit[0].point;
        }
        return Vector2.zero;
        //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
        //return hitPos;
    }
    protected Vector2 WallLeftPosition(Vector2 startPoint)
    {
        int hitCount = 0;
        //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
        RaycastHit2D[] hit = Cast(startPoint, Vector2.left, 100f, true, out hitCount, true);
        if (hit != null)
        {
            Debug.DrawRay(startPoint, hit[0].point);
            return hit[0].point;
        }
        return Vector2.zero;
        //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
        //return hitPos;
    }
    private static ContactFilter2D m_contactFilter;
    private static RaycastHit2D[] m_hitResults;
    private static bool m_isInitialized;

    private static void Initialize()
    {
        if (m_isInitialized == false)
        {
            m_contactFilter.useLayerMask = true;
            m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
            //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
            m_hitResults = new RaycastHit2D[16];
            m_isInitialized = true;
        }
    }
    protected static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
    {
        Initialize();
        m_contactFilter.useTriggers = !ignoreTriggers;
        hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
        if (debugMode)
        {
            if (hitCount > 0)
            {
                Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
            }
            else
            {
                Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
            }
        }
#endif
        return hitCount == 0 ? null : m_hitResults;
    }
    void Start()
    {
        Vector2 northdirection = RoofPosition(transform.position);
        Vector2 southdirection = FloorPosition(transform.position);
        Vector2 eastdirection = WallRightPosition(transform.position);
        Vector2 westdirection = WallLeftPosition(transform.position);
        float distance1 = Vector2.Distance(northdirection, transform.position);
        float distance2 = Vector2.Distance(southdirection, transform.position);
        float distance3 = Vector2.Distance(eastdirection, transform.position);
        float distance4 = Vector2.Distance(westdirection, transform.position);
        if (distance1 < distance2)
        {

            if (distance1 < distance3)
            {
                if (distance1 < distance4)
                {
                    m_toCreate = Object.Instantiate(m_toCreate);
                    northdirection.y= northdirection.y - 2.5f;
                    m_toCreate.transform.localPosition = northdirection;
                    m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {

                        m_toCreate = Object.Instantiate(m_toCreate);
                        westdirection.x = westdirection.x + 2.5f;
                        m_toCreate.transform.localPosition = westdirection;
                        m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 270);

                }


            }
            else
            {

                if (distance3 < distance4)
                {

                    m_toCreate = Object.Instantiate(m_toCreate);
                    eastdirection.x = eastdirection.x - 2.5f;
                    m_toCreate.transform.localPosition = eastdirection;
                    m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 90);

                }
                else
                {
                    m_toCreate = Object.Instantiate(m_toCreate);
                    westdirection.x = westdirection.x + 2.5f;
                    m_toCreate.transform.localPosition = westdirection;
                    m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 270);


                }

            }

        }
        else
        {

            if (distance2 < distance3)
            {
                if (distance2 < distance4)
                {
                    m_toCreate = Object.Instantiate(m_toCreate);
                    southdirection.y = southdirection.y + 2.5f;
                    m_toCreate.transform.localPosition = southdirection;
                    m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 0);


                }
                else
                {
                        m_toCreate = Object.Instantiate(m_toCreate);
                        westdirection.x = westdirection.x + 2.5f;
                        m_toCreate.transform.localPosition = westdirection;
                        m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 270);
                }


            }
            else
            {
                if (distance3 < distance4)
                {
                    m_toCreate = Object.Instantiate(m_toCreate);
                    eastdirection.x = eastdirection.x - 2.5f;
                    m_toCreate.transform.localPosition = eastdirection;
                    m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 90);


                }
                else
                {
                    m_toCreate = Object.Instantiate(m_toCreate);
                    westdirection.x = westdirection.x + 2.5f;
                    m_toCreate.transform.localPosition = westdirection;
                    m_toCreate.transform.rotation = Quaternion.Euler(0, 0, 270);


                }


            }

        }
       
    }
    

    }
