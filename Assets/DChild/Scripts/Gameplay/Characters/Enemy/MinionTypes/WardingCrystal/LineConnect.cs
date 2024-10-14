using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild;

public class LineConnect : MonoBehaviour
{
    private LineRenderer linerenderer;
    private Transform m_Root;
    private Transform m_Target;
    private bool Activated = false;
    [SerializeField]
    private GameObject m_TargetVFX;
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

        m_TargetVFX.transform.position = m_Target.transform.position;
    }

    public void SetupLineConnect(Transform Root, Transform Target)
    {
        m_Root = Target;
        m_Target = Root;
        Activated = true;
    }
}
