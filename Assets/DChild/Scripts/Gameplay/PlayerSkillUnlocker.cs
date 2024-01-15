using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using PixelCrushers.DialogueSystem;
using Holysoft.Event;
using System.Collections;
using UnityEngine.Timeline;


namespace DChild.Gameplay
{
    public class PlayerSkillUnlocker : MonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isUsed;

            public SaveData(bool isUsed) : this()
            {
                m_isUsed = isUsed;
            }
            public bool isUsed => m_isUsed;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isUsed);
        }

        [SerializeField, FoldoutGroup("Has Skill Indicator")]
        private GameObject m_platformGlow;
        [SerializeField, FoldoutGroup("Has Skill Indicator")]
        private GameObject m_leftStatueGlow;
        [SerializeField, FoldoutGroup("Has Skill Indicator")]
        private GameObject m_rightStatueGlow;

        [SerializeField]
        private PrimarySkill m_toUnlock;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private PlayableDirector m_cinematic;
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private GameplaySignalHandle m_shrineSignalHandle;
        [SerializeField]
        private SignalReceiver m_signalReceiver;
        [SerializeField]
        private SignalAsset[] m_signalsToRun;
        [SerializeField, OnValueChanged("OnIsUsedChanged")]
        private bool m_isUsed;
        [SerializeField, LuaScriptWizard(true)]
        private string m_onInteractionCommand;
        [SerializeField]
        SkillShrineVisualHandle m_shrineVisualHandle;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

        public string promptMessage => "Use";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public ISaveData Save() => new SaveData(m_isUsed);

        public void Load(ISaveData data)
        {
            m_isUsed = ((SaveData)data).isUsed;
            m_collider.enabled = !m_isUsed;
            SetGlows(!m_isUsed);
            m_shrineVisualHandle.SkillShrineState(m_isUsed);
        }

        public void Initialize()
        {
            m_isUsed = false;
            m_collider.enabled = true;
            SetGlows(true);
            m_shrineVisualHandle.SkillShrineState(m_isUsed);

        }

        public void Interact(Character character)
        {
            if (m_isUsed == false)
            {
                if (character)
                {
                    character.GetComponent<PlayerControlledObject>().owner.skills.SetSkillStatus(m_toUnlock, true);

                }

                if (m_cinematic == null)
                {
                    GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(m_toUnlock);
                    m_shrineVisualHandle.SkillShrineState(true);
                }
                else
                {
                    m_cinematic.Play();
                }

                m_isUsed = true;
                m_collider.enabled = false;
                Lua.Run(m_onInteractionCommand);
            }
        }

        private void OnCutsceneDone(PlayableDirector obj)
        {
            StartCoroutine(OnCutsceneEnded());
        }

        private IEnumerator OnCutsceneEnded()
        {
            //makes sure cutscene has ended before calling notifyskill  
            yield return new WaitForSeconds(1f);
            NotifySkill(m_toUnlock);
            SetGlows(false);
            m_shrineVisualHandle.SkillShrineState(false);
            for(int i = 0; i < m_signalsToRun.Length; i++)
            {
                m_signalReceiver.GetReaction(m_signalsToRun[i]).Invoke();
            }
        }

        private void NotifySkill(PrimarySkill skill)
        {
            GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(skill);
        }

        private void SetGlows(bool isOn)
        {
            m_platformGlow.SetActive(isOn);
            m_leftStatueGlow.SetActive(isOn);
            m_rightStatueGlow.SetActive(isOn);
        }

        private void Awake()
        {
            m_cinematic.stopped += OnCutsceneDone;
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }

#if UNITY_EDITOR
        private void OnIsUsedChanged()
        {
            m_collider.enabled = !m_isUsed;
            SetGlows(!m_isUsed);
        }

        [Button]
        private void Interact()
        {
            Interact(null);
        }
#endif
    }
}