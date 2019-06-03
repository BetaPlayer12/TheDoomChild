using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public abstract class SecretPassageBlockade : DestructableObject
    {
        [System.Serializable]
        public struct Serializer
        {
            [System.Serializable]
            public struct Data
            {
                [SerializeField]
                private bool m_isOpen;

                public Data(bool isOpen)
                {
                    this.m_isOpen = isOpen;
                }

                public bool isOpen => m_isOpen;
            }

            [SerializeField]
            [ReadOnly]
            private SecretPassageBlockade m_blockade;
            [SerializeField]
            private Data m_serializedData;

            public Serializer(SecretPassageBlockade blockade)
            {
                m_blockade = blockade;
                m_serializedData = new Data(false);
            }

            public Data serializedData => m_serializedData;

            public static Serializer[] CreateSerializers()
            {
                var blockadeList = (SecretPassageBlockade[])Resources.FindObjectsOfTypeAll(typeof(SecretPassageBlockade));
                var blockade = new SecretPassageBlockade.Serializer[blockadeList.Length];
                for (int i = 0; i < blockade.Length; i++)
                {
                    blockade[i] = new SecretPassageBlockade.Serializer(blockadeList[i]);
                }
                return blockade;
            }

            public void Save() => m_serializedData = m_blockade.Save();

            public void Load() => m_blockade.Load(m_serializedData);
        }

        [SerializeField]
        private ParticleFX m_damageFX;
        protected bool m_isOpen;

        public Serializer.Data Save() => new Serializer.Data(m_isOpen);
        public abstract void Load(Serializer.Data load);
    }
}