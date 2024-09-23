using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnect : MonoBehaviour
{
    private LineRenderer linerenderer;
    private Transform m_Root;
    private Transform m_Target;
    private bool Activated = false;
    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        linerenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Activated)
        {
            return;
        }
        linerenderer.SetPosition(0, m_Root.position);
        linerenderer.SetPosition(1, m_Target.position);
    }

    public void SetupLineConnect(Transform Root, Transform Target)
    {
        m_Root = Root;
        m_Target = Target;
        Activated = true;
    }
}
