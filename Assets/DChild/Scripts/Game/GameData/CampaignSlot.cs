using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using Holysoft.Collections;
using DChild.Gameplay.Environment;
using UnityEditor;

namespace DChild.Serialization
{
    [System.Serializable]
    public class CampaignSlot
    {
        [SerializeField, ReadOnly, BoxGroup("Slot Info")]
        private int m_id;
        [SerializeField, OnValueChanged("OnNewGameChange"), BoxGroup("Slot Info")]
        private bool m_demoGame;
        [SerializeField, OnValueChanged("OnNewGameChange"), BoxGroup("Slot Info")]
        private bool m_newGame;
        [SerializeField, BoxGroup("Slot Info")]
        private SceneInfo m_sceneToLoad;
        [SerializeField, HideIf("m_newGame"), BoxGroup("Slot Info")]
        private Location m_location;
        [SerializeField, HideIf("m_newGame"), BoxGroup("Slot Info")]
        private SerializedVector2 m_spawnPosition;
        [SerializeField, HideIf("m_newGame"), MinValue(0), BoxGroup("Slot Info")]
        private int m_completion;
        [SerializeField, HideIf("m_newGame"), BoxGroup("Slot Info")]
        private TimeKeeper m_duration;

        [SerializeField, HideIf("m_newGame")]
        private PlayerCharacterData m_characterData;
        [SerializeField, HideReferenceObjectPicker, HideIf("m_newGame")]
        private SerializeDataList m_campaignProgress;
        [SerializeField, HideReferenceObjectPicker, HideIf("m_newGame")]
        private SerializeDataList m_zoneDatas;

        public CampaignSlot(int m_id)
        {
            this.m_id = m_id;
            m_newGame = true;
            m_location = Location.None;
            m_completion = 0;
            m_duration = new TimeKeeper();
        }

        public CampaignSlot()
        {
            this.m_id = 0;
            m_newGame = true;
            m_location = Location.None;
            m_completion = 0;
            m_duration = new TimeKeeper();
            m_characterData = new PlayerCharacterData();
            m_campaignProgress = new SerializeDataList();
            m_zoneDatas = new SerializeDataList();
        }

        public int id => m_id;
        public bool demoGame => m_demoGame;
        public bool newGame => m_newGame;
        public SceneInfo sceneToLoad => m_sceneToLoad;
        public Location location => m_location;
        public int completion => m_completion;
        public TimeKeeper duration => m_duration;

        public Vector2 spawnPosition { get => m_spawnPosition; }
        public PlayerCharacterData characterData => m_characterData;


        public void Reset()
        {
            m_newGame = true;
            m_location = m_demoGame ? Location.Garden : Location.None;
            m_completion = 0;
            m_duration = new TimeKeeper();
        }

        public void UpdateLocation(SceneInfo scene, Location location, Vector2 spawnPosition)
        {
            m_sceneToLoad = scene;
            m_location = location;
            m_spawnPosition = spawnPosition;
        }

        public void UpdateCharacterData(PlayerCharacterData data)
        {
            m_characterData = data;
        }

        public void UpdateCampaignProgress(SerializeDataID ID, ISaveData saveData) => m_campaignProgress.UpdateZoneData(ID, saveData);

        public T GetCampaignProgress<T>(SerializeDataID ID) where T : ISaveData => (T)m_campaignProgress.GetZoneData(ID);

        public void UpdateZoneData(SerializeDataID ID, ISaveData saveData) => m_zoneDatas.UpdateZoneData(ID, saveData);

        public T GetZoneData<T>(SerializeDataID ID) where T : ISaveData => (T)m_zoneDatas.GetZoneData(ID);

#if UNITY_EDITOR
        public void SetID(int ID)
        {
            m_id = ID;
        }

        private void OnNewGameChange()
        {
            if (m_newGame)
            {
                Reset();
            }
        }
#endif
    }
}