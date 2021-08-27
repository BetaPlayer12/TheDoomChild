using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using Doozy.Engine.UI;

namespace DChild.Gameplay.UI
{
    public class RoomText : MonoBehaviour, ISerializableComponent
    {
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_Shown;

            public SaveData(bool m_Shown)
            {
                this.m_Shown = m_Shown;
            }

            public bool Shown => m_Shown;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_Shown);
        }

        [SerializeField]
        private bool m_Shown;
        [SerializeField]
        private UIView m_UI;
        [SerializeField, MinValue(0)]
        private float m_TextFadeDelay;

        public ISaveData Save()
        {
            return new SaveData(m_Shown);
        }

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            m_Shown = saveData.Shown;

        }
        private IEnumerator DelayedFade()
        {
            yield return new WaitForSeconds(m_TextFadeDelay);
            m_UI.Hide();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_Shown != true)
            {
                if (collision.isTrigger)
                {
                    if (collision.TryGetComponent(out Hitbox check))
                    {
                        //show animation
                        m_UI.Show();
                        m_Shown = true;
                        StartCoroutine(DelayedFade());
                    }
                }
            }
        }


        private void OnValidate()
        {
            DChildUtility.ValidateSensor(gameObject);
        }
    }

}