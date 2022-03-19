using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdaptiveScrollbar : MonoBehaviour
{
    [SerializeField, MinValue(1), OnValueChanged("UpdateHandleVisuals")]
    private int m_divideListBy = 1;
    [SerializeField, MinValue(1), OnValueChanged("UpdateHandleVisuals")]
    private int m_visibleItemCount = 1;
    [ShowInInspector, MinValue(1), OnValueChanged("UpdateHandleVisuals")]
    private int m_maxListCount = 1;
    private Scrollbar m_scrollBar;

    public void SetMaxItemCount(int maxItemCount)
    {
        m_maxListCount = maxItemCount;
        UpdateHandleVisuals();
    }

    private void UpdateHandleVisuals()
    {
#if UNITY_EDITOR
        if (m_scrollBar == null)
        {
            m_scrollBar = GetComponent<Scrollbar>();
        }
#endif
        var visisbleItems = m_visibleItemCount - 1;
        m_scrollBar.size = (float)m_divideListBy/ (m_maxListCount - visisbleItems);
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_scrollBar = GetComponent<Scrollbar>();
        UpdateHandleVisuals();
    }

}
