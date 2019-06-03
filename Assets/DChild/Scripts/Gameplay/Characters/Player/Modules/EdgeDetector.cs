using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

public class EdgeDetector : MonoBehaviour, IPlayerExternalModule
{
    public class Modules
    {
        private IPlayerModules source;

        public Modules(IPlayerModules source)
        {
            this.source = source;
        }

        public IPlacementState state => source.characterState;
        public HorizontalDirection currentFacingDirection => source.currentFacingDirection;
        public CharacterPhysics2D physics => source.physics;
        public RaySensor edgeSensor => source.sensors.edgeSensor;
    }

    [SerializeField]
    private float m_velocityLimit;
    private Modules m_modules;


    public void Initialize(IPlayerModules player)
    {
        m_modules = new Modules(player);
    }

    private void Update()
    {
        if (m_modules.state.isGrounded)
        {
            m_modules.edgeSensor.Cast();    
            m_modules.state.isNearEdge = (m_modules.edgeSensor.isDetecting) ? false : true;

            //if (m_modules.state.isNearEdge)
            //{
            //    var input = Input.GetAxisRaw("Horizontal");
            //    if ((m_modules.currentFacingDirection == Direction.Left && input < 0) ||
            //        (m_modules.currentFacingDirection == Direction.Right && input > 0))
            //    {
            //        if (m_modules.physics.velocity.x > -m_velocityLimit && m_modules.physics.velocity.x < m_velocityLimit)
            //        {
            //            var value = (m_modules.currentFacingDirection == Direction.Left) ? -0.5f : 0.5f;
            //            var fallPos = m_modules.physics.transform.position;
            //            fallPos.x += value;
            //            m_modules.physics.transform.position = Vector2.Lerp(m_modules.physics.transform.position, fallPos, 5f);
            //        }
            //    }
            //}
        }
    }

    public void Start()
    {
        m_velocityLimit = 20f;
    }
}

