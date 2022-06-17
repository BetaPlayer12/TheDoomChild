using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{
    public class SoulSkillChangeArea : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                GameplaySystem.soulSkillManager.AllowSoulSkillActivation(true);

            }

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                GameplaySystem.soulSkillManager.AllowSoulSkillActivation(false);
            }
        }

    }
}
