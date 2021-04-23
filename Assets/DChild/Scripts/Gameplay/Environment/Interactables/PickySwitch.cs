/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [AddComponentMenu("DChild/Gameplay/Environment/Interactable/Switches/Picky Switch")]
    public class PickySwitch : Switch
    {
        [SerializeField, TabGroup("Main", "Restrictions")]
        private string[] m_viableTags;

        public override bool CanBeInteractedWith(Collider2D collider2D)
        {
            if (base.CanBeInteractedWith(collider2D))
            {
                if (m_viableTags.Length > 0)
                {
                    for (int i = 0; i < m_viableTags.Length; i++)
                    {
                        if (collider2D.CompareTag(m_viableTags[i]))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }
    }
}