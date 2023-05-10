using DChild.Gameplay.Characters.Players;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Gameplay.UI.CombatArts
{
    public class CombatArtUIDetail : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_artNameLabel;
        [SerializeField]
        private VideoPlayer m_preview;
        [SerializeField]
        private TextMeshProUGUI m_descriptionLabel;

        public void Display(CombatArtData data, int level)
        {
            if (data != null)
            {
                m_artNameLabel.text = data.combatArtName;
                if (level > 1)
                {
                    m_artNameLabel.text += $" {level}";
                }
                Display(data.GetCombatArtLevelData(level));
            }
        }

        private void Display(CombatArtLevelData levelData)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayPreview(levelData.preview));
            m_descriptionLabel.text = levelData.description;
        }

        private IEnumerator DisplayPreview(VideoClip clip)
        {
            m_preview.Stop();
            yield return null;
            m_preview.clip = clip;
            m_preview.Play();
        }
    }

}