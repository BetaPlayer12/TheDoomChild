using TMPro;
using UnityEngine;

namespace DChild.Gameplay.FastTravel
{
    public class FastTravelOptionButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_buttonLabel;
        [SerializeField]
        private FastTravelData m_data;

        public void SetData(FastTravelData data)
        {
            m_data = data;
            if (m_data)
            {
                m_buttonLabel.text = m_data.pointName;
            }
        }
    }
}
