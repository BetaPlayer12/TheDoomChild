/***************************************************
 * 
 *   performs grass cut effect as it takes damage.
 *  no action yet upon it's Death
 * 
 **************************************************/
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    //[RequireComponent(typeof(AdvanceFoliage))]
    public class Grass : DestructableObject
    {
        [System.Serializable]
        public struct Serializer
        {
            [System.Serializable]
            public struct Data
            {
                [SerializeField]
                private bool m_isCut;

                public Data(bool isCut)
                {
                    this.m_isCut = isCut;
                }

                public bool isCut => m_isCut;
            }

            [SerializeField]
            [ReadOnly]
            private Grass m_grass;
            [SerializeField]
            private Data m_serializedData;

            public Serializer(Grass grass)
            {
                m_grass = grass;
                m_serializedData = new Data(false);
            }

            public Data serializedData => m_serializedData;

            public static Serializer[] CreateSerializers()
            {
                var grassList = (Grass[])Resources.FindObjectsOfTypeAll(typeof(Grass));
                var grass = new Grass.Serializer[grassList.Length];
                for (int i = 0; i < grass.Length; i++)
                {
                    grass[i] = new Grass.Serializer(grassList[i]);
                }
                return grass;
            }

            public void Save() => m_serializedData = m_grass.Save();

            public void Load() => m_grass.Load(m_serializedData);
        }

        [SerializeField]
        private GameObject cutGrass;

        [SerializeField]
        private GameObject m_cutFX;

        //private AdvanceFoliage m_foilage;
        private List<Foliage> m_foilage;  // to be replaced
        private Material m_originalMaterial;

        private bool m_isCut;
        public override Vector2 position => transform.position;
        public override IAttackResistance attackResistance => null;

        public void Load(Serializer.Data data)
        {
            m_isCut = data.isCut;
            if (m_isCut)
            {
                StartAsCut();
            }
            else
            {
                RevertToUncut();
            }

            StopAllCoroutines();
        }

        public Serializer.Data Save() => new Serializer.Data(m_isCut);

        public override void Heal(int health)
        {
        }

        public override void TakeDamage(int totalDamage, AttackType type) => BecomeCutGrass();

        private void RevertToUncut()
        {
            m_isCut = false;
            //var renderers = m_foilage.renderer;
            //renderers[0].sharedMaterial = m_originalMaterial;
            //for (int i = 1; i < renderers.Length; i++)
            //{
            //    renderers[i].gameObject.SetActive(true);
            //}
            for(int i = m_foilage.Count - 1; i >= 0; i--)
            {
                m_foilage[i].gameObject.SetActive(true);
            }

            cutGrass.SetActive(false);
            EnableHitboxes();
        }

        [Button("Cut")]
        private void BecomeCutGrass()
        {
            var fx = GameplaySystem.fXManager.InstantiateFX<ParticleFX>(m_cutFX, position);
            fx.Play();
            StartAsCut();
        }

        private void StartAsCut()
        {
            m_isCut = true;
            //m_foilage.ResetState();
            //var renderers = m_foilage.renderers;
            //renderers[0].sharedMaterial = Database.GetDatabase<GrassDatabase>().GetMaterial();
            //for (int i = 1; i < renderers.Length; i++)
            //{
            //    renderers[i].gameObject.SetActive(false);
            //}
            for (int i = m_foilage.Count - 1; i >= 0; i--)
            {
                m_foilage[i].gameObject.SetActive(false);
            }
            cutGrass.SetActive(true);
            DisableHitboxes();
        }

        private void Start()
        {
            m_foilage = new List<Foliage>(GetComponentsInChildren<Foliage>());
            cutGrass.SetActive(false);
            //m_foilage = GetComponent<AdvanceFoliage>();
            //m_originalMaterial = m_foilage.renderers[0].sharedMaterial;
        }

       
    }
}
