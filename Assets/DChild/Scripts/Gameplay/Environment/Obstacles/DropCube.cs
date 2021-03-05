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
    [SerializeField]
    private float m_speed;
    [SerializeField]
    public bool m_isDropping;
    [SerializeField]
    public bool dropped;
    Collider m_Collider;
    [Button]
    public void Drop()
    {
        m_isDropping = true;
    }


    public void SetLerpAs(bool drop)
    {
        if (drop)
        {
            SetMoveValues(m_cube.localPosition, m_StartPosition);
        }
        else
        {
            SetMoveValues(m_cube.localPosition, m_EndPosition);
        }
    }
    private void SetMoveValues(Vector2 start, Vector2 destination)
    {
        m_start = start;
        m_destination = destination;
    }
    public void Lerp(float lerpValue)
    {
        m_cube.localPosition = Vector2.Lerp(m_start, m_destination, lerpValue);
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
        if (m_isDropping == true)
        {
            if (dropped == false)
            {
                SetMoveValues(m_cube.localPosition, m_EndPosition);
                m_cube.localPosition = Vector2.Lerp(m_start, m_destination, m_speed);
                if (Mathf.Approximately(m_cube.localPosition.y, m_EndPosition.y) && Mathf.Approximately(m_cube.localPosition.x, m_EndPosition.x))
                {
                    dropped = true;
                }
            }
            if (dropped == true)
            {
                SetMoveValues(m_cube.localPosition, m_StartPosition);
                m_cube.localPosition = Vector2.Lerp(m_start, m_destination, m_speed);
                if (Mathf.Approximately(m_cube.localPosition.y, m_StartPosition.y) && Mathf.Approximately(m_cube.localPosition.x, m_StartPosition.x))
                {
                    m_isDropping = false;
                    dropped = false;
                    m_cubecrusher.GetComponent<Collider2D>().enabled = false;
                }
            }

        }

    }
}
