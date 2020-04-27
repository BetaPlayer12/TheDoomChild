using DChild.Gameplay;
using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public class AnimatedSwitchHandle : ISwitchHandle
    {
        [SerializeField, MinValue(0.1)]
        private float m_transitionDelay;
        [SerializeField, HideReferenceObjectPicker, TabGroup("Enter")]
        private UnityEvent m_onEntrance = new UnityEvent();
        [SerializeField, HideReferenceObjectPicker, TabGroup("Exit")]
        private UnityEvent m_onExit = new UnityEvent();

        private static Scene m_originalScene;

        public float transitionDelay => m_transitionDelay;

        public bool needsButtonInteraction => true;

        public Vector3 promptPosition => Vector3.zero;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            switch (type)
            {
                case TransitionType.Enter:
                    m_onEntrance?.Invoke();
                    break;
                case TransitionType.PostEnter:
                    character.gameObject.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(character.gameObject, m_originalScene);
                    break;
                case TransitionType.Exit:
                    m_onExit?.Invoke();
                    break;
                case TransitionType.PostExit:
                    //character.gameObject.transform.parent = null;
                    //SceneManager.MoveGameObjectToScene(character.gameObject, m_originalScene);
                    break;
            }
        }
    }
}
