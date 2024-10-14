using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.Players;
using PixelCrushers.DialogueSystem.Wrappers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleScenarioIntroHandle
    {
        private enum Type
        {
            None = 0,
            Dialogue,
            Cinematic
        }

        [SerializeField]
        private Type type;
        [SerializeField, ShowIf("@type == Type.Dialogue")]
        private DialogueSystemTrigger m_dialogueTrigger;
        [SerializeField, ShowIf("@type == Type.Cinematic")]
        private PlayableDirector m_cinematic;

        public void Execute()
        {
            switch (type)
            {
                case Type.None:
                    ArmyBattleSystem.StartBattleGameplay();
                    break;
                case Type.Dialogue:
                    m_dialogueTrigger.OnUse();
                    break;
                case Type.Cinematic:
                    m_cinematic.Play();
                    break;
            }

        }
    }
}
