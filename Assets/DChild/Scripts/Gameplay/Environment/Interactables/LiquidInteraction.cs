using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidInteraction : MonoBehaviour
{
    [SerializeField]
    private Material m_material;

    private List<Vector4> m_rippleLocations;

    private float m_rippleInterval;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Hitbox")
        {
            GetPlayerLocation(collision.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Hitbox")
        {
            if (Mathf.Abs(collision.gameObject.GetComponentInParent<Rigidbody2D>().velocity.x) > 3)
            {
                if (m_rippleInterval > .25f)
                {
                    m_rippleInterval = 0;
                    GetPlayerLocation(collision.transform);
                }
                else
                {
                    m_rippleInterval += Time.deltaTime;
                }
            }
        }
    }

    private void GetPlayerLocation(Transform transform)
    {

        if (m_rippleLocations.Count == 10)
        {
            m_rippleLocations.Clear();
        }

        m_rippleLocations.Add(Vector4.zero);
        var calc1 = transform.position.x * 0.025f;
        var calc2 = calc1 + .5f;
        var ripplePos = new Vector2(calc2, m_material.GetVector("_RippleLocation_" + (m_rippleLocations.Count).ToString()).y);
        m_rippleLocations[m_rippleLocations.Count - 1] = ripplePos;
        m_material.SetVector("_RippleLocation_" + (m_rippleLocations.Count).ToString(), m_rippleLocations[m_rippleLocations.Count - 1]);

        StartCoroutine(RippleModeRoutine(m_rippleLocations.Count));
    }

    private IEnumerator RippleModeRoutine(int count)
    {
        var time = -0.001f;

        while (time <= 1.8f)
        {
            m_material.SetFloat("_RippleTime_" + (count).ToString(), time += GameplaySystem.time.deltaTime);
            yield return null;
        }
        m_material.SetFloat("_RippleTime_" + (count).ToString(), -0.001f);
        yield return null;
    }

    private void Start()
    {
        m_material.SetInt("_RippleAutoTime", 0);
    }

    private void Awake()
    {
        m_rippleLocations = new List<Vector4>();
    }
}
