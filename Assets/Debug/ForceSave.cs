using UnityEngine;

namespace DChildDebug
{
    public class ForceSave : MonoBehaviour
    {
        [SerializeField]
        private CampaignSlotData[] m_toSave;

        private void Awake()
        {
            for (int i = 0; i < m_toSave.Length; i++)
            {
                m_toSave[i].SaveToFile();
            }
        }
    } 
}