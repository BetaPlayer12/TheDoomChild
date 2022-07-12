using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Environment;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class MovableObjectPrompt : MonoBehaviour
    {
        [SerializeField]
        private ObjectManipulation m_detector;
        [SerializeField]
        private RectTransform m_prompt;

        private MovableObject m_promptFor;

        private void OnMovableObjectDetected(object sender, DetectedMovableObject eventArgs)
        {
            if (eventArgs.isEmpty)
            {
                GameplaySystem.gamplayUIHandle.ShowMovableObjectPrompt(false);
                m_promptFor = null;
                enabled = false;
            }
            else
            {
                m_promptFor = eventArgs.movableObject;
                enabled = true;
                GameplaySystem.gamplayUIHandle.ShowMovableObjectPrompt(true);
            }
        }

        private void Awake()
        {
            m_detector.MovableObjectDetected += OnMovableObjectDetected;
        }

        private void Update()
        {
            if(m_promptFor != null)
            {
                m_prompt.position =  m_promptFor.promptPosition;
            }
        }
    }
}