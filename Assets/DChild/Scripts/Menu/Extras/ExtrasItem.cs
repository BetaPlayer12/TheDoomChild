using UnityEngine;

namespace DChild.Menu.Extras
{
    public class ExtrasItem : MonoBehaviour
    {
        [SerializeField]
        protected int m_id;

        public int id => m_id;

        public void SetID(int id) => m_id = id;
    }
}