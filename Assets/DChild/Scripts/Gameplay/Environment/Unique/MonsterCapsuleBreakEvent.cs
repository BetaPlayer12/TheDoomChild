using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class MonsterCapsuleBreakEvent : SerializedMonoBehaviour
    {
        [SerializeField]
        private Transform m_capsuleCenter;
        [SerializeField]
        private Transform m_monsterLocation;
        //[SerializeField]
        //private SpineAnimation m_monsterAnimation;
        [SerializeField]
        private SpineAnimation m_glassBreakAnimation;
        [SerializeField]
        private GameObject m_nonAnimatedGlass;
        [SerializeField]
        private GameObject m_fixedVersion;
        [SerializeField]
        private GameObject m_brokenVersion;
        [SerializeField]
        private GameObject m_spillFX;
        [SerializeField]
        private IAmbushingAI m_monster;

        private ICombatAIBrain m_brain;

        public void ChangeToBrokenState()
        {
            StopAllCoroutines();
            m_fixedVersion.SetActive(false);
            //m_monsterAnimation.gameObject.SetActive(false);
            m_glassBreakAnimation.gameObject.SetActive(false);
            m_brokenVersion.SetActive(true);
            //m_monster.gameObject.SetActive(true);
        }

        [Button]
        public void ExecuteMonsterEscape()
        {
            StopAllCoroutines();
            StartCoroutine(MonsterEscapeRoutine());
            StartCoroutine(GlassBreakRoutine());
        }

        private IEnumerator MonsterEscapeRoutine()
        {
            //m_monsterAnimation.SetAnimation(0, "Escape", false);
            //yield return new WaitForAnimationComplete(m_monsterAnimation.animationState, "Escape");
            //m_monster.gameObject.SetActive(true);
            m_brain?.SetTarget(GameplaySystem.playerManager.player.damageableModule);
            Vector2 location = new Vector2(m_monsterLocation.transform.position.x, m_monsterLocation.transform.position.y);
            m_monster?.LaunchAmbush(location);
            yield return null;
        }

        private IEnumerator GlassBreakRoutine()
        {
            this.InstantiateToScene(m_spillFX, m_capsuleCenter.position, Quaternion.identity);
            m_nonAnimatedGlass.SetActive(false);
            m_glassBreakAnimation.gameObject.SetActive(true);
            m_glassBreakAnimation.SetAnimation(0, "Break", false);
            yield return new WaitForSeconds(4.5f);
            m_fixedVersion.SetActive(false);

        }

        public void Reset()
        {
            StopAllCoroutines();
            m_fixedVersion.SetActive(true);
            m_nonAnimatedGlass.SetActive(true);
            //m_monsterAnimation.gameObject.SetActive(true);
            m_glassBreakAnimation.gameObject.SetActive(false);
            m_brokenVersion.SetActive(false);
            //m_monster.gameObject.SetActive(false);
            m_glassBreakAnimation.SetEmptyAnimation(0, 0);
            Vector2 location = new Vector2(m_monsterLocation.transform.position.x, m_monsterLocation.transform.position.y);
            m_monster?.PrepareAmbush(location);
        }

        private void Awake()
        {
            Reset();
        }
    }
}