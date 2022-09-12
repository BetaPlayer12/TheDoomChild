using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Environment
{
    public class BaseIllusionPlatform : IllusionPlatform
    {
        [Button]
        public override void Appear(bool instant)
        {
            gameObject.SetActive(true);
        }

        [Button]
        public override void Disappear(bool instant)
        {
            gameObject.SetActive(false);
        }
    }
}
