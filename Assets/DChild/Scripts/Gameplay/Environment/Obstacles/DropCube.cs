using Holysoft;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCube : MonoBehaviour
{
    [SerializeField]
    private Transform m_cube;
    [SerializeField]
    private Transform m_cubecrusher;
    [SerializeField, HorizontalGroup("Start"), ShowIf("m_cube")]
    private Vector2 m_StartPosition;
    [SerializeField, HorizontalGroup("End"), ShowIf("m_cube")]
    private Vector2 m_EndPosition;
    // Start is called before the first frame update
    private Vector2 m_start;
    private Vector2 m_destination;
    private Vector2 m_currentPos;
    private bool m_shake = false;
    [SerializeField]
    private AnimationCurve m_speedCurve;
    [SerializeField]
    private float m_Fallspeed;
    [SerializeField]
    private float m_Returnspeed;
    [SerializeField]
    public bool m_isDropping;
    [SerializeField]
    public bool dropped;
    [SerializeField]
    public int delay;
    [SerializeField]
    private float m_radiusOffset = 1;
    [Button]
    public void Drop()
    {
        m_shake = true;
        StartCoroutine(DelayCoroutine());
        transform.position = m_currentPos;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
           
            StartCoroutine(DelayCoroutine());
            m_isDropping = false;
            dropped = true;
           
        }
    }
    IEnumerator DelayCoroutine()
    {
        m_currentPos.x = transform.position.x;
        m_currentPos.y = transform.position.y;
        yield return new WaitForSeconds(delay);
        m_shake = false;
        m_isDropping = true;
    }
   
    private void SetMoveValues(Vector2 start, Vector2 destination)
    {
        m_start = start;
        m_destination = destination;
    }
   
   private Vector2 RoundVectorValuesTo(uint decimalPlace, Vector2 vector2)
    {
        return new Vector2(MathfExt.RoundDecimalTo(decimalPlace, vector2.x), MathfExt.RoundDecimalTo(decimalPlace, vector2.y));
    }

#if UNITY_EDITOR


    [ResponsiveButtonGroup("Start/Button"), Button("Use Current"), ShowIf("m_cube")]
    private void UseCurrentForStartPosition()
    {
        m_StartPosition = m_cube.localPosition;
    }

    [ResponsiveButtonGroup("End/Button"), Button("Use Current"), ShowIf("m_cube")]
    private void UseCurrentForEndPosition()
    {
        m_EndPosition = m_cube.localPosition;
    }
#endif
   
    public void Update()
    {
        var offset = Random.insideUnitCircle;
        if (m_shake == true)
        {
            transform.position = m_currentPos + offset * m_radiusOffset;
        }
        if (m_isDropping == true)
        {
            if (dropped == false)
            {
              
                SetMoveValues(m_cube.localPosition, m_EndPosition);
               m_cube.localPosition = Vector3.MoveTowards(m_start, m_destination, m_Fallspeed);
               
            }
            if (dropped == true)
            {
                SetMoveValues(m_cube.localPosition, m_StartPosition);
                m_cube.localPosition = Vector3.MoveTowards(m_start, m_destination, m_Returnspeed);
              
                if(RoundVectorValuesTo(2, m_cube.localPosition) == RoundVectorValuesTo(2, m_StartPosition))
                {
                    m_isDropping = false;
                    dropped = false;
                    m_cubecrusher.GetComponent<Collider2D>().enabled = false;
                }
            }

        }
       
    }
}
