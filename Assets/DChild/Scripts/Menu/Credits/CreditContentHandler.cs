using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu
{
    public class CreditContentHandler : UIBehaviour
    {
        [SerializeField]
        private RectTransform m_visibleRect;
        [SerializeField]
        [ReadOnly]
        private CreditContent[] m_contents;
        private List<CreditContent> m_toCheckForEnable;
        private List<CreditContent> m_toCheckForDisable;
        private ICreditReel m_eventSender;

        private void OnStop(object sender, EventActionArgs eventArgs)
        {
            m_toCheckForEnable.Clear();
            m_toCheckForEnable.AddRange(m_contents);
            m_toCheckForDisable.Clear();
        }

        private void OnPlay(object sender, EventActionArgs eventArgs)
        {
            enabled = true;
        }

        private void OnPause(object sender, EventActionArgs eventArgs)
        {
            enabled = false;
        }

        private void CheckForEnableRects()
        {
            for (int i = m_toCheckForEnable.Count - 1; i >= 0; i--)
            {
                if (m_toCheckForEnable[i].rectTransform.Overlaps(m_visibleRect))
                {
                    m_toCheckForEnable[i].Show();
                    m_toCheckForDisable.Add(m_toCheckForEnable[i]);
                    m_toCheckForEnable.RemoveAt(i);
                }
            }
        }

        public void CheckForDisableRects()
        {
            for (int i = m_toCheckForDisable.Count - 1; i >= 0; i--)
            {
                if (m_toCheckForDisable[i].rectTransform.Overlaps(m_visibleRect) == false)
                {
                    m_toCheckForDisable[i].Hide();
                    m_toCheckForDisable.RemoveAt(i);
                }
            }
        }

        private void Awake()
        {
            m_toCheckForEnable = new List<CreditContent>(m_contents);
            m_toCheckForDisable = new List<CreditContent>();
            m_eventSender = GetComponentInChildren<ICreditReel>();
            m_eventSender.CreditsPause += OnPause;
            m_eventSender.CreditsPlay += OnPlay;
            m_eventSender.CreditsStop += OnStop;
        }

        private void Start()
        {
            for (int i = 0; i < m_contents.Length; i++)
            {
                m_contents[i].Hide();
            }
            CheckForEnableRects();
        }

        private void Update()
        {
            CheckForEnableRects();
            CheckForDisableRects();
        }

        private void OnValidate()
        {
            m_contents = GetComponentsInChildren<CreditContent>();
        }
    }
}