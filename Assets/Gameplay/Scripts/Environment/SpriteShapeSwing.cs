using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeSwing : MonoBehaviour
{

    //public GameObject m_Prefab;
    //public Vector3 offset;
    //protected int index = 1;
    //public float speed = 2f;
    //protected Spline splineshape;
    //public Vector3 Radius = new Vector3(0.2f, 0.02f, 0f);

    //protected Vector3 pointPosition;
    //GameObject go;

    //void Start()
    //{
    //    SetupSpriteShapePositions();  
    //}

    //void SetupSpriteShapePositions()
    //{
    //    splineshape = GetComponent<SpriteShapeController>().spline;
    //    pointPosition = splineshape.GetPosition(index);
    //    if (m_Prefab != null)
    //    {
    //        go = GameObject.Instantiate(m_Prefab);
    //        go.transform.parent = gameObject.transform;
    //        //go.transform.position = splineshape.GetPosition(index);
    //    }  
    //}

    //void Update()
    //{

    //    UpdateSpriteShapePosition();
    //}

    //void UpdateSpriteShapePosition()
    //{
    //    Vector3 newPos = new Vector3(Radius.x * Mathf.Cos(speed * Time.time), Radius.y * Mathf.Sin(speed * Time.time), 0.0f);
    //    var point = new Vector3(pointPosition.x + newPos.x, pointPosition.y + newPos.y);
    //    splineshape.SetPosition(index, point);

    //    if (go != null)
    //    {
    //        var pos = gameObject.transform.position + splineshape.GetPosition(index) + offset;
    //        go.transform.position = pos;
    //    }
    //}

    
}
