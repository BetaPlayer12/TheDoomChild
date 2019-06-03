using Holysoft.UI;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Holysoft.Menu
{
    public abstract class PanelNavigationHandler<T> : UIBehaviour where T : INavigationPanel
    {
        [SerializeField]
        protected bool m_useDefaultPanelOnEnable;
        [SerializeField, ShowIf("m_useDefaultPanelOnEnable"), ValueDropdown("DefaulPanelDropdown"), Indent]
        protected int m_defaultPanel;

        protected T m_active;
        protected bool m_hasActivePanel;

        protected abstract T[] panels { get; }

        protected void Open(int index)
        {
            if (m_hasActivePanel)
            {
                m_active.Close();
            }
            m_active = panels[index];
            m_hasActivePanel = true;
            m_active.Open();
        }

        public override void Enable()
        {
            base.Enable();
            if (m_useDefaultPanelOnEnable)
            {
                if (m_hasActivePanel)
                {
                    m_active.ForceClose();
                }
                m_active = panels[m_defaultPanel];
                m_hasActivePanel = true;
                m_active.ForceOpen();
            }
        }

        protected virtual void Start()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].ForceClose();
            }
            if (m_useDefaultPanelOnEnable)
            {
                m_active = panels[m_defaultPanel];
                m_active.ForceOpen();
            }
        }

#if UNITY_EDITOR
        [SerializeField, DisplayAsString, PropertyOrder(1), LabelText("Active Panel")]
        protected string m_activePanelName;

        protected virtual void OnDefaultPanelChange()
        {
            m_defaultPanel = Mathf.Clamp(m_defaultPanel, 0, panels.Length - 1);
        }

        protected virtual void OnPanelChange()
        {
            if (panels == null)
            {
                m_defaultPanel = 0;
                m_activePanelName = "None";
            }
        }

        private IEnumerable DefaulPanelDropdown()
        {
            var list = new ValueDropdownList<int>();
            for (int i = 0; i < panels.Length; i++)
            {
                list.Add(panels[i].name, i);
            }

            return list;
        }
#endif
    }
}
