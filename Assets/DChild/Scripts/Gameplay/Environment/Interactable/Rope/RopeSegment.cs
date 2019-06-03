using UnityEngine;
using DChild.Gameplay;
using DChild.Gameplay.Environment.Interractables;

namespace DChild.Gameplay.Environment.Interractables
{
    public class RopeSegment : MonoBehaviour, IInteractable
    {

        public Vector3 position => transform.position;
        private Rigidbody2D m_rigidbody;
        public new Rigidbody2D rigidbody
        {
            get { return m_rigidbody; }
        }

        public IInteractable Interact(IInteractingAgent agent)
        {
            agent.transform.SetParent(transform);
            var body = agent.GetComponent<IsolatedPhysics2D>();
            body.Disable();
            return this;
        }

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }
    } 
}
