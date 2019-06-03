/************************************
 * 
 * A Cutscene is Played when player
 * is inside the trigger
 * 
 ************************************/

using System;
using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    [RequireComponent(typeof(Collider2D))]
    [DisallowMultipleComponent]
    public class CutsceneTrigger : MonoBehaviour
    {
        [System.Serializable]
        public struct Serializer
        {
            [System.Serializable]
            public struct Data
            {
                [SerializeField]
                private bool m_isTriggered;

                public Data(bool isTriggered)
                {
                    this.m_isTriggered = isTriggered;
                }

                public bool isTriggered => m_isTriggered;
            }

            [SerializeField]
            [ReadOnly]
            private CutsceneTrigger m_director;
            [SerializeField]
            private Data m_serializedData;

            public Serializer(CutsceneTrigger m_director) : this()
            {
                this.m_director = m_director;
            }

            public void Load() => m_director.Load(m_serializedData);
            public void Save() => m_serializedData = m_director.Save();
        }

        [SerializeField]
        private Cutscene m_cutscene;
        private bool m_isTriggered;


        public void Load(Serializer.Data serializedData)
        {
            m_isTriggered = serializedData.isTriggered;
            GetComponent<Collider2D>().enabled = m_isTriggered;
        }

        public Serializer.Data Save() => new Serializer.Data(m_isTriggered);

        private Player GetPlayer(Collider2D collision)
        {
            if (collision.tag == "Hitbox")
            {
                return collision.GetComponentInParent<Player>();
            }
            else
            {
                return null;
            }
        }

        private void Awake()
        {
            m_cutscene.CutsceneEnd += OnCutsceneEnd;
        }

        private void OnCutsceneEnd(object sender, EventActionArgs eventArgs)
        {
            m_isTriggered = true;
            GetComponent<Collider2D>().enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = GetPlayer(collision);
            if (player)
            {
                m_cutscene.Play();
            }
        }

        private void OnValidate()
        {
            var collider = GetComponent<Collider2D>();
            if (collider)
            {
                collider.isTrigger = true;
            }
        }
    }

}