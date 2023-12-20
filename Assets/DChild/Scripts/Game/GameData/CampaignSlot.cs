using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using Holysoft.Collections;
using DChild.Gameplay.Environment;
using UnityEditor;
using PixelCrushers.DialogueSystem;
using DLocation = DChild.Gameplay.Environment.Location;

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
        [SerializeField]
        private bool m_allowWriteToDisk;
        [SerializeField, BoxGroup("Slot Info")]
        private SceneInfo m_sceneToLoad;
        [SerializeField, HideIf("m_newGame"), BoxGroup("Slot Info")]
        private DLocation m_location;
        [SerializeField, HideIf("m_newGame"), BoxGroup("Slot Info")]
        private SerializedVector2 m_spawnPosition;
        [SerializeField, HideIf("m_newGame"), MinValue(0), BoxGroup("Slot Info")]
        private int m_completion;
        [SerializeField, HideIf("m_newGame"), BoxGroup("Slot Info"), ClockTime]
        private float m_duration;

        [SerializeField, HideIf("m_newGame")]
        private PlayerCharacterData m_characterData;
        [SerializeField, HideReferenceObjectPicker, HideIf("m_newGame"), TabGroup("Campaign")]
        private SerializeDataList m_campaignProgress;
        [SerializeField, HideReferenceObjectPicker, HideIf("m_newGame"), TabGroup("Zone")]
        private SerializeDataList m_zoneDatas;
        [SerializeField, DrawWithUnity, TabGroup("ZoneSlots Importable")]
        private ZoneSlot[] m_importable;
        [Button, TabGroup("ZoneSlots Importable")]
        public void Import()
        {
            for (int i = 0; i < m_importable.Length; i++)
            {
                m_zoneDatas.UpdateData(m_importable[i].id, ((ISaveData)m_importable[i].zoneDatas).ProduceCopy());
            }
        }
        [SerializeField, HideReferenceObjectPicker, HideIf("m_newGame"), TabGroup("Misc")]
        private SerializeDataList m_miscDatas;
        [SerializeField, HideReferenceObjectPicker, TextArea(5, 999)]
        private string m_dialogueSaveData;

        public CampaignSlot(int m_id)
        {
            this.m_id = m_id;
            m_newGame = false;
            m_allowWriteToDisk = true;
            m_location = DLocation.None;
            m_spawnPosition = new SerializedVector2();
            m_completion = 0;
            m_duration = 0;
            m_characterData = new PlayerCharacterData();
            m_campaignProgress = new SerializeDataList();
            m_zoneDatas = new SerializeDataList();
            m_miscDatas = new SerializeDataList();
            m_dialogueSaveData = null;
        }

        public CampaignSlot()
        {
            this.m_id = 1;
            m_newGame = false;
            m_allowWriteToDisk = true;
            m_location = DLocation.None;
            m_spawnPosition = new SerializedVector2();
            m_completion = 0;
            m_duration = 0;
            m_characterData = new PlayerCharacterData();
            m_campaignProgress = new SerializeDataList();
            m_zoneDatas = new SerializeDataList();
            m_miscDatas = new SerializeDataList();
            m_dialogueSaveData = null;
        }

        public int id => m_id;
        public bool demoGame => m_demoGame;
        public bool newGame => m_newGame;
        public bool allowWriteToDisk => m_allowWriteToDisk;
        public SceneInfo sceneToLoad => m_sceneToLoad;
        public DLocation location => m_location;
        public int completion => m_completion;
        public float duration => m_duration;

        public Vector2 spawnPosition { get => m_spawnPosition; }
        public PlayerCharacterData characterData => m_characterData;

        public SerializeDataList campaignProgress => m_campaignProgress;
        public SerializeDataList zoneDatas => m_zoneDatas;
        public string dialogueSaveData => m_dialogueSaveData;
        public ZoneSlot[] Importable => m_importable;


        [Button]
        public void Reset()
        {
            m_newGame = false;
            m_location = m_demoGame ? DLocation.City_Of_The_Dead : DLocation.None;
            m_spawnPosition = new SerializedVector2();
            m_spawnPosition.x = -1209f;
            m_spawnPosition.y = 90f;
            m_completion = 0;
            m_duration = 0;
            m_characterData = new PlayerCharacterData();
            m_campaignProgress = new SerializeDataList();
            m_zoneDatas = new SerializeDataList();
            m_miscDatas = new SerializeDataList();
            m_dialogueSaveData = null;
        }

        public void Copy(CampaignSlot slot)
        {
            m_demoGame = slot.demoGame;
            m_newGame = slot.newGame;
            m_location = slot.location;
            m_sceneToLoad = slot.sceneToLoad;
            m_spawnPosition = slot.spawnPosition;
            m_completion = slot.completion;
            m_duration = slot.duration;
            m_characterData = new PlayerCharacterData(slot.characterData);
            m_campaignProgress = new SerializeDataList(slot.campaignProgress);
            m_zoneDatas = new SerializeDataList(slot.zoneDatas);
            m_dialogueSaveData = slot.dialogueSaveData;
        }

        public void UpdateLocation(SceneInfo scene, DLocation location, Vector2 spawnPosition)
        {
            m_sceneToLoad = scene;
            m_location = location;
            m_spawnPosition = spawnPosition;
        }

        public void UpdateDuration(float value) => m_duration = Mathf.Max(0, value);

        public void UpdateCharacterData(PlayerCharacterData data) => m_characterData = data;

        public void UpdateCampaignProgress(SerializeID ID, ISaveData saveData) => m_campaignProgress.UpdateData(ID, saveData);

        public T GetCampaignProgress<T>(SerializeID ID) where T : ISaveData => (T)m_campaignProgress.GetData(ID);

        public void UpdateZoneData(SerializeID ID, ISaveData saveData) => m_zoneDatas.UpdateData(ID, saveData);

        public T GetZoneData<T>(SerializeID ID) where T : ISaveData => (T)m_zoneDatas.GetData(ID);

        public void UpdateData(SerializeID ID, ISaveData saveData) => m_miscDatas.UpdateData(ID, saveData);

        public T GetData<T>(SerializeID ID) where T : ISaveData => (T)m_miscDatas.GetData(ID);

        public void UpdateDialogueSaveData() => m_dialogueSaveData = PersistentDataManager.GetSaveData();

        public void SetAsNewGame(bool isNewGame) => m_newGame = isNewGame;

        #region EditorOnly
#if UNITY_EDITOR
        public CampaignSlot(CampaignSlot slot)
        {
            this.m_id = slot.id;
            Copy(slot);
        }

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
        #endregion
    }
}