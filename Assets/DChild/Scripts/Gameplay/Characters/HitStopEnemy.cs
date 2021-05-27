using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopEnemy : MonoBehaviour
{
    [SerializeField]
    private float m_whiteDecayTime;
    [SerializeField]
    private bool m_enableHitStop;
    [SerializeField, TabGroup("Reference")]
    protected SpineRootAnimation m_animation;
    [SerializeField, TabGroup("Reference")]
    private List<Collider2D> m_hitboxColliders;
    [SerializeField, TabGroup("Modules")]
    private FlinchHandler m_flinchHandle;
    [SerializeField, TabGroup("Renderer")]
    private MeshRenderer m_Rendererer;

    private MaterialPropertyBlock m_propertyBlock;
    private HitStopHandle m_hitstop;

    private void OnHitStopStart(object sender, EventActionArgs eventArgs)
    {
        Debug.Log("HitStop");
        StopAllCoroutines();
        //StartCoroutine(FlinchWhiteRoutine());
        if (m_enableHitStop)
        {
            foreach (Collider2D collider in m_hitboxColliders)
            {
                if (collider.IsTouchingLayers(LayerMask.GetMask("Player")))
                {
                    m_hitstop.Execute();
                    break;
                }
            }
        }
    }

    private IEnumerator FlinchWhiteRoutine()
    {
        m_propertyBlock.SetFloat("Highlight", 1);
        m_Rendererer.SetPropertyBlock(m_propertyBlock);
        float highLightCurrentValue = 1;
        while (highLightCurrentValue > 0.01f)
        {
            highLightCurrentValue -= Time.deltaTime * m_whiteDecayTime;
            m_propertyBlock.SetFloat("Highlight", highLightCurrentValue);
            m_Rendererer.SetPropertyBlock(m_propertyBlock);
            yield return null;
        }
        m_propertyBlock.SetFloat("Highlight", 0);
        m_Rendererer.SetPropertyBlock(m_propertyBlock);
        yield return null;
    }

    private void Start()
    {
        m_Rendererer.GetPropertyBlock(m_propertyBlock);
    }

    private void Awake()
    {
        m_propertyBlock = new MaterialPropertyBlock();
        m_flinchHandle.HitStopStart += OnHitStopStart;
        m_hitstop = GetComponent<HitStopHandle>();
    }
}
