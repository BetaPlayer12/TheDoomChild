using DChild.Menu.Bestiary;
using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChildDebug.Menu.Bestiary
{
    public class BestiaryProgressInjector : SerializedMonoBehaviour
    {
        public BestiaryPage m_page;
        [OnValueChanged("UpdateProgress")]
        public BestiaryList m_list;
        [HideReferenceObjectPicker]
        public Dictionary<int, bool> m_progress = new Dictionary<int, bool>();

        private void Start()
        {
            m_page.SetProgress(new BestiaryProgress(m_progress));
        }

        private void UpdateProgress()
        {
            if (m_list)
            {
                m_progress.Clear();
                var bestiaryIDs = m_list.GetIDs();
                for (int i = 0; i < bestiaryIDs.Length; i++)
                {
                    m_progress.Add(bestiaryIDs[i], true);
                }
            }
            else
            {
                m_progress.Clear();
            }
        }
    }
}