using Sirenix.OdinInspector;
using UnityEngine;
using USpine = Spine.Unity;

namespace DChild.Gameplay.ArmyBattle.Units
{
    public class ArmyUnitMelee : ArmyUnit
    {
        [SerializeField, USpine.SpineAnimation]
        private string[] m_moveAnimations;

        public override DamageType type => DamageType.Melee;

        [Button]
        public void Move(Vector2 direction)
        {
            var signedDirection = Mathf.Sign(direction.x);
            var signedScale = Mathf.Sign(transform.localScale.x);

            if (signedDirection != signedScale)
            {
                transform.localScale = new Vector3(signedDirection, 1, 1);
            }

            PlayRandomAnimation(m_moveAnimations, true);
        }

    }
}