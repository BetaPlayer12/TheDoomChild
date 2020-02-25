using Holysoft.UI;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Holysoft.Collections
{
    public class SpineAnimationCue : UIBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_skeletonData;

        [SpineAnimation(dataField: "m_skeletonData")]
        public List<string> animations;

        [SerializeField]
        private UICanvas m_transitionLabel;

        [SerializeField]
        private TextMeshProUGUI m_text;

        [SerializeField]
        private float cue;

        [SerializeField]
        private float speed;

        private bool isCalled;

        private ImageColorLerpAnimation m_highlightLabelOpacity;

        private void Start()
        {
            m_highlightLabelOpacity = GetComponentInChildren<ImageColorLerpAnimation>();
            this.enabled = true;
            isCalled = false;
        }

        private void LateUpdate()
        {
            if (m_skeletonData?.AnimationState.GetCurrent(0).ToString() == animations[0])
            {
                var spineLength = m_skeletonData.AnimationState.GetCurrent(0).TrackTime;
                if (Mathf.Abs(spineLength) >= cue)
                {
                    m_text.alpha = Mathf.Lerp(m_text.color.a, 0f, speed * Time.deltaTime);
                    m_highlightLabelOpacity.Play();
                    if (m_text.alpha <= 0f)
                    {
                        m_transitionLabel.Hide();
                        enabled = false;
                    }
                }
            }
        }
    }
}
