using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnDemoUnityEvent : MonoBehaviour
{
    public string purposeOfScript;

    [SerializeField]
    private Image m_demoEndPanel;
    [SerializeField]
    private TextMeshProUGUI m_demoEndText;

    [SerializeField]
    private UnityEvent m_onDemoEnd;

    private void Awake()
    {
        m_demoEndPanel.color = new Color(0, 0, 0, 0);
        m_demoEndText.color = new Color(1, 1, 1, 0);
        m_demoEndPanel.gameObject.SetActive(false);
    }

    public void EndDemo()
    {
        m_onDemoEnd?.Invoke();
    }

    public void ShowEndScreen()
    {
        if (GameplaySystem.campaignSerializer.slot.demoGame)
        {
            m_demoEndPanel.gameObject.SetActive(true);
            StartCoroutine(FadeInRoutine());
        }
    }

    private IEnumerator FadeInRoutine()
    {
        yield return new WaitForSeconds(3f);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            m_demoEndPanel.color = new Color(0, 0, 0, i);
            m_demoEndText.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    
}
