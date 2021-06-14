using DChild.Gameplay;
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
    public Vector2 m_startPosition;
    [SerializeField, HorizontalGroup("End"), ShowIf("m_cube")]
    public Vector2 m_endPosition;
    // Start is called before the first frame update
    private Vector2 m_start;
    private Vector2 m_destination;
    private Vector2 m_currentPos;
    private bool m_shake = false;
    [SerializeField]
    private AnimationCurve m_fallSpeedCurve;
    [SerializeField]
    private AnimationCurve m_returnSpeedCurve;
    private float m_fallTime;
    private float m_returnTime;
    private float m_fallSpeed;
    [SerializeField]
    private float m_maxFallSpeed;
    private float m_returnSpeed;
    [SerializeField]
    private float m_maxReturnSpeed;
    private float m_currentFallSpeed;
    private float m_currentReturnSpeed;
    public bool m_isDropping;
    public bool dropped;
    public bool m_istriggered = false;
    [SerializeField]
    public float m_FallDelay;
    [SerializeField]
    public float m_ReturnDelay;
    [SerializeField]
    private float m_radiusOffset = 1;
    [Button]
    public void Drop()
    {
        if (m_istriggered == false)
        {
            m_fallTime = 0;
            m_returnTime = 0;
            m_shake = true;
            StartCoroutine(FallDelayCoroutine());
            transform.position = m_currentPos;
            m_istriggered = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
           
            StartCoroutine(ReturnDelayCoroutine());
            transform.position = m_currentPos;
            m_isDropping = false;
            dropped = true;
           
        }
    }
    IEnumerator FallDelayCoroutine()
    {
        m_currentPos.x = transform.position.x;
        m_currentPos.y = transform.position.y;
        yield return new WaitForSeconds(m_FallDelay);
        m_shake = false;
        m_isDropping = true;
        
    }
    IEnumerator ReturnDelayCoroutine()
    {
        m_currentPos.x = transform.position.x;
        m_currentPos.y = transform.position.y;
        yield return new WaitForSeconds(m_ReturnDelay);
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
        m_startPosition = m_cube.localPosition;
    }

    [ResponsiveButtonGroup("End/Button"), Button("Use Current"), ShowIf("m_cube")]
    private void UseCurrentForEndPosition()
    {
        m_endPosition = m_cube.localPosition;
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
            var deltaTime = GameplaySystem.time.deltaTime;
            if (dropped == false)
            {
                m_fallTime += deltaTime;
                m_fallSpeed = m_fallSpeedCurve.Evaluate(m_fallTime);
                m_currentFallSpeed = m_maxFallSpeed * m_fallSpeed;
                SetMoveValues(m_cube.localPosition, m_endPosition);
               m_cube.localPosition = Vector3.MoveTowards(m_start, m_destination, m_currentFallSpeed* deltaTime);
               
            }
            if (dropped == true)
            {
                m_returnTime += deltaTime;
                m_returnSpeed = m_returnSpeedCurve.Evaluate(m_returnTime);
                m_currentReturnSpeed = m_maxReturnSpeed * m_returnSpeed;
                SetMoveValues(m_cube.localPosition, m_startPosition);
                m_cube.localPosition = Vector3.MoveTowards(m_start, m_destination, m_currentReturnSpeed* deltaTime);
              
                if(RoundVectorValuesTo(2, m_cube.localPosition) == RoundVectorValuesTo(2, m_startPosition))
                {
                    m_isDropping = false;
                    dropped = false;
                    m_istriggered = false;
                    m_cubecrusher.GetComponent<Collider2D>().enabled = false;
                }
            }

        }
       
    }
}
