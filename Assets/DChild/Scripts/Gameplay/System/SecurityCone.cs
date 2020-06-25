using UnityEngine;

namespace DChild.Gameplay
{
    public class SecurityCone : MonoBehaviour
    {
        [SerializeField]
        private Transform m_teleportEntitiesHere;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out Character character))
            {
                //Do something to prevent awkward transistioning before this line
                character.transform.position = m_teleportEntitiesHere.position;
            }
        }
    }

}