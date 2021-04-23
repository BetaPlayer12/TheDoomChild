using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Environment;
using PlayerNew;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public class AnimatedSwitchHandle : ISwitchHandle
    {
        [SerializeField]
        private Transform m_newParent;
        [SerializeField, MinValue(0.1)]
        private float m_transitionDelay;
        [SerializeField, HideReferenceObjectPicker, TabGroup("Enter")]
        private UnityEvent m_onEntrance = new UnityEvent();
        [SerializeField, HideReferenceObjectPicker, TabGroup("Exit")]
        private UnityEvent m_onExit = new UnityEvent();

        private static Scene m_originalScene;
        private static Vector2 m_parentLocalPosition;
        private static Transform m_originalParent;

        public float transitionDelay => m_transitionDelay;

        public bool needsButtonInteraction => true;

        public Vector3 promptPosition => Vector3.zero;

        public string prompMessage => null;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            CharacterState collisionState = character.GetComponentInChildren<CharacterState>();
            
            switch (type)
            {
                case TransitionType.Enter:
                    m_originalParent = character.transform.parent;
                    if (m_originalParent == null)
                    {
                        m_originalScene = character.gameObject.scene;
                    }
                    character.transform.parent = m_newParent;
                    m_parentLocalPosition = character.transform.localPosition;
                    m_onEntrance?.Invoke();
                    break;

                case TransitionType.PostEnter:
                    character.gameObject.transform.parent = m_originalParent;
                    if (m_originalParent == null)
                    {
                        SceneManager.MoveGameObjectToScene(character.gameObject, m_originalScene);
                    }
                    rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                    collisionState.forcedCurrentGroundedness = true;
                    break;

                case TransitionType.Exit:
                    character.transform.parent = m_newParent;
                    character.transform.localPosition = m_parentLocalPosition;
                    rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    collisionState.forcedCurrentGroundedness = false;
                    m_onExit?.Invoke();
                    break;

                case TransitionType.PostExit:
                    character.gameObject.transform.parent = m_originalParent;
                    if (m_originalParent == null)
                    {
                        SceneManager.MoveGameObjectToScene(character.gameObject, m_originalScene);
                    }
                    break;
            }
        }
    }
}
