using UnityEngine;
using System.Collections;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusIllusionPlatform : MonoBehaviour
    {
        [SerializeField]
        private BaseIllusionPlatform m_normalPlatform;
        [SerializeField]
        private BaseIllusionPlatform m_spikedPlatform;
        [SerializeField]
        private float m_spikeTransformDelay;
        [SerializeField]
        private float m_spikeFormDuration;

        private bool m_isInSpikeForm;

        [Button]
        public void Show()
        {
            m_normalPlatform.gameObject.SetActive(true);
            m_spikedPlatform.gameObject.SetActive(true);

            m_isInSpikeForm = false;
            m_normalPlatform.Appear(false);
            m_spikedPlatform.Disappear(true);
        }

        [Button]
        public void Hide()
        {
            StopAllCoroutines();
            m_normalPlatform.Disappear(true);
            m_spikedPlatform.Disappear(true);
        }

        private IEnumerator TransformationToSpikeRoutine()
        {
            yield return new WaitForSeconds(m_spikeTransformDelay);
            m_isInSpikeForm = true;
            m_normalPlatform.Disappear(false);
            m_spikedPlatform.Appear(false);

            yield return new WaitForSeconds(m_spikeFormDuration);

            m_normalPlatform.Appear(false);
            m_spikedPlatform.Disappear(false);
            m_isInSpikeForm = false;
        }

        private void Start()
        {
            m_normalPlatform.gameObject.SetActive(false);
            m_spikedPlatform.gameObject.SetActive(false);
            m_isInSpikeForm = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_isInSpikeForm)
                return;

            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null && playerObject.owner == (IPlayer)GameplaySystem.playerManager.player)
            {
                StartCoroutine(TransformationToSpikeRoutine());
            }
        }
    }
}
