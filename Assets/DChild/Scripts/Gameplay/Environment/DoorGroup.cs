using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class DoorGroup : MonoBehaviour
    {
        [SerializeField, SceneObjectsOnly]
        private Door[] m_doors;

        public void Open()
        {
            for (int i = 0; i < m_doors.Length; i++)
            {
                m_doors[i].Open();
            }
        }

        public void Close()
        {
            for (int i = 0; i < m_doors.Length; i++)
            {
                m_doors[i].Close();
            }
        }

        public void ToggleState()
        {
            for (int i = 0; i < m_doors.Length; i++)
            {
                m_doors[i].ToggleState();
            }
        }
    }

}