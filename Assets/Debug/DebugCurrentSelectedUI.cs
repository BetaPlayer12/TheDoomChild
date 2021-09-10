using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugCurrentSelectedUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_currentSelected;

    // Update is called once per frame
    void Update()
    {
        m_currentSelected = EventSystem.current.currentSelectedGameObject;
    }
}
