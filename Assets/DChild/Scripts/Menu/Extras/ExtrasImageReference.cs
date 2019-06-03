using Holysoft.Collections;
using UnityEngine;

namespace DChild.Menu.Extras
{
    public class ExtrasImageReference : ReferenceFactoryAssembler, IReferenceFactoryData
    {
        [SerializeField]
        private SpriteList m_list;

        public int instanceCount => m_list.count;

        protected override void OnInstanceCreated(object sender, ReferenceInstanceEventArgs eventArgs)
        {
            var extrasImage = eventArgs.value.GetComponent<ExtrasImage>();
            extrasImage.SetSprite(m_list.GetSprite(eventArgs.referenceIndex));
        }
    }
}