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
    private GameObject m_SourcePoint;
    [SerializeField]
    private GameObject m_TargetPoint;
    private GameObject m_SourceVFX;
    private GameObject m_TargetVFX;
    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        linerenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_SourcePoint, gameObject.scene);

        m_SourceVFX = instance.gameObject;

        instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_TargetVFX, gameObject.scene);

        m_TargetVFX = instance.gameObject;
    }

    private void OnDestroy()
    {
        Destroy(m_SourceVFX);
        Destroy(m_TargetVFX);
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

        m_Target.transform.position = m_Target.position;
        m_SourceVFX.transform.position = m_Root.position;
    }

    public void SetupLineConnect(Transform Root, Transform Target)
    {
        m_Root = Root;
        m_Target = Target;
        Activated = true;
    }
}
