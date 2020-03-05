using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Serialization;
using DChild.Menu;
using Holysoft.Event;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay
{
    public class ZoneMoverHandle : MonoBehaviour, IGameplaySystemModule
    {
        [SerializeField, Min(0.1f)]
        private float m_entranceWaitTime;
        [SerializeField, Min(0.1f)]
        private float m_exitWaitTime;

        [SerializeField]
        private Vector2 m_popVelocity;
        [SerializeField, Min(1)]
        private float m_flightVelocity;

        private Coroutine m_routine;
        private bool m_isSceneDone;

        //public void MoveCharacterToLocation(Character character, LocationData location, TravelDirection entranceType)
        //{
        //    if (m_routine != null)
        //    {
        //        StopCoroutine(m_routine);
        //    }
        //    m_routine = StartCoroutine(Routine(character, location, entranceType));
        //}

        private void OnSceneDone(object sender, EventActionArgs eventArgs)
        {
            m_isSceneDone = true;
        }

        //private IEnumerator Routine(Character character, LocationData location, TravelDirection entranceType)
        //{
        //    yield return null;
        //    //var currentVelocity = character.physics.velocity;
        //    var controller = GameplaySystem.playerManager.OverrideCharacterControls();
        //    //character.physics.SetVelocity(currentVelocity);
        //    var damageable = character.GetComponent<IDamageable>();
        //    damageable.SetHitboxActive(false);
        //    //switch (entranceType)
        //    //{
        //    //    case TravelDirection.Right:
        //    //        controller.moveDirectionInput = 1;
        //    //        break;
        //    //    case TravelDirection.Left:
        //    //        controller.moveDirectionInput = -1;
        //    //        break;
        //    //}

        //    //if (entranceType == TravelDirection.Top)
        //    //{
        //    //    float time = m_entranceWaitTime;
        //    //    do
        //    //    {
        //    //        time -= Time.deltaTime;
        //    //        character.physics.SetVelocity(Vector2.up * m_flightVelocity);
        //    //        yield return new WaitForFixedUpdate();
        //    //    } while (time > 0);
        //    //}
        //    //else
        //    //{
        //    //    yield return new WaitForSeconds(m_entranceWaitTime);
        //    //}

        //    //controller.moveDirectionInput = 0;
        //    //currentVelocity = character.physics.velocity;
        //    //character.physics.SetVelocity(Vector2.zero);
        //    //character.physics.simulateGravity = false;
        //    //var groundednessHandle = character.GetComponentInChildren<GroundednessHandle>();
        //    //groundednessHandle.enabled = false;
        //    m_isSceneDone = false;
        //    LoadingHandle.SetLoadType(LoadingHandle.LoadType.Smart);
        //    GameSystem.LoadZone(location.scene, true);

        //    //while (m_isSceneDone == false)
        //    //{
        //    //    yield return null;
        //    //}
        //    character.transform.position = location.position;
        //    //character.physics.SetVelocity(currentVelocity);
        //    //character.physics.simulateGravity = true;
        //    //groundednessHandle.enabled = true;


        //    //switch (location.exitDirection)
        //    //{
        //    //    case TravelDirection.Right:
        //    //        controller.moveDirectionInput = 1;
        //    //        yield return new WaitForSeconds(m_exitWaitTime);
        //    //        controller.moveDirectionInput = 0;
        //    //        break;
        //    //    case TravelDirection.Left:
        //    //        controller.moveDirectionInput = -1;
        //    //        yield return new WaitForSeconds(m_exitWaitTime);
        //    //        controller.moveDirectionInput = 0;
        //    //        break;
        //    //    case TravelDirection.Top:
        //    //        controller.enabled = false;
        //    //        var characterPhysics = character.GetComponent<CharacterPhysics2D>();
        //    //        characterPhysics.SetVelocity(Vector2.zero);
        //    //        var exitVelocity = m_popVelocity;
        //    //        exitVelocity.x *= (int)character.facing;
        //    //        characterPhysics.AddForce(exitVelocity, ForceMode2D.Impulse);
        //    //        while (characterPhysics.onWalkableGround == false)
        //    //        {
        //    //            yield return null;
        //    //        }
        //    //        break;
        //    //    case TravelDirection.Bottom:
        //    //        var physics = character.GetComponent<CharacterPhysics2D>();
        //    //        physics.SetVelocity(Vector2.zero);
        //    //        while (physics.onWalkableGround == false)
        //    //        {
        //    //            yield return null;
        //    //        }
        //    //        break;
        //    //}
        //    damageable.SetHitboxActive(true);
        //    //GameplaySystem.playerManager.StopCharacterControlOverride();
        //}

        private void Awake()
        {
            LoadingHandle.LoadingDone += OnSceneDone;
        }
    }
}