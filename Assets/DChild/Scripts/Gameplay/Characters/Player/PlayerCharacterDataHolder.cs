using Sirenix.OdinInspector;
using Holysoft.Collections;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    [InfoBox("This will only Hold Player Character Data Stuff and not do anything about it")]
    public class PlayerCharacterDataHolder<T> : ISerializable<T>
    {
        private T m_data;

        public void LoadData(T data)
        {
            m_data = data;
        }

        public T SaveData()
        {
            return m_data;
        }
    }
}