using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DChildEditor
{
    public class MasterAudioToolKitWindow : OdinEditorWindow
    {
        private static MasterAudioToolKitWindow m_instance;

        [MenuItem("Tools/Kit/Master Audio ToolKit")]
        private static void OpenWindow()
        {
            m_instance = EditorWindow.GetWindow<MasterAudioToolKitWindow>();
            m_instance.Show();
        }

        [SerializeField, ValidateInput("ValidateDSGC")]
        private GameObject m_dynamicSoundGroupCreator;

        [SerializeField, MinValue(1)]
        private int m_voices = 1;

        [SerializeField, ValueDropdown("GetDynamicSoundGroups", IsUniqueList = true)]
        private DynamicSoundGroup[] m_soundGroups;

        private bool ValidateDSGC(GameObject reference)
        {
            if (reference == null)
            {
                return false;
            }

            if (reference.TryGetComponent(out DynamicSoundGroupCreator dsgc))
            {
                return true;
            }
            else
            {
                reference = null;
                return false;
            }
        }

        private IEnumerable GetDynamicSoundGroups() => m_dynamicSoundGroupCreator.GetComponentsInChildren<DynamicSoundGroup>();

        [Button, ShowIf("@m_soundGroups.Length > 0")]
        private void ChangeVoices()
        {
            for (int i = 0; i < m_soundGroups.Length; i++)
            {
                m_soundGroups[i].voiceLimitCount = m_voices;

            }
        }
    }

}