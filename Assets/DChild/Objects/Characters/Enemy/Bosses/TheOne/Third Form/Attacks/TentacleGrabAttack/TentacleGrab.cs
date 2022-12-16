using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TentacleGrab : MonoBehaviour
{

    [SerializeField, TabGroup("Reference")]
    protected SpineRootAnimation m_animation;
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_emergeAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_anticipationLoopAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_grabAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_defaultRetractAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_grabbingAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_groundSlamAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_groundSlamRetractAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_waitForInputAnimation;

    [SerializeField]
    private Transform m_tentacleGrabHand;
    [SerializeField]
    private GameObject m_dummyPlayer;
    [SerializeField]
    private Collider2D m_grabHitbox;

    private bool isAttackDone = false;
    [SerializeField]
    private bool isPlayerGrabbed = false;

    [SerializeField]
    private PlayableDirector m_groundSlamTimelineCall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void GroundSlamAttack()
    {        
        StartCoroutine(GroundSlam());        
    }

    private IEnumerator Emerge()
    {
        m_grabHitbox.enabled = false;
        m_animation.SetAnimation(0, m_emergeAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_emergeAnimation);
        m_animation.SetAnimation(0, m_anticipationLoopAnimation, true);
        yield return null;
    }

    private IEnumerator DefaultRetract()
    {
        m_grabHitbox.enabled = false;
        m_animation.SetAnimation(0, m_defaultRetractAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_defaultRetractAnimation);
    }

    private IEnumerator Grab()
    {
        m_grabHitbox.enabled = true;
        m_animation.SetAnimation(0, m_grabAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_grabAnimation);
        yield return null;
    }

    private IEnumerator GroundSlamRetract()
    {
        m_grabHitbox.enabled = false;
        m_dummyPlayer.SetActive(false);
        GameplaySystem.playerManager.player.gameObject.SetActive(true);
        GameplaySystem.playerManager.player.character.gameObject.SetActive(true);

        m_animation.SetAnimation(0, m_groundSlamRetractAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_groundSlamRetractAnimation);
    }

    private IEnumerator ReturnGrabTentacleToWaitForInput()
    {
        m_animation.SetAnimation(0, m_waitForInputAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_waitForInputAnimation);
    }

    private IEnumerator GroundSlam()
    {
        yield return Emerge();

        yield return new WaitForSeconds(2f); //somehow make time tentacle grabs random interval?

        yield return Grab();

        if (isPlayerGrabbed)
        {
            GameplaySystem.playerManager.DisableControls();
            if (m_groundSlamTimelineCall != null)
            {
                yield return TimelineGroundSlamSequence();
            }
            else
            {
                yield return HardcodedGroundSlamSequence();
            }

            GameplaySystem.playerManager.EnableControls();
            GameplaySystem.playerManager.player.transform.parent = null;
            isAttackDone = false;
            yield return null;
        }
        else
        {
            yield return DefaultRetract();
        }

        isPlayerGrabbed = false;
    }

    private IEnumerator TimelineGroundSlamSequence()
    {
        GameplaySystem.playerManager.player.transform.SetParent(m_tentacleGrabHand);
        GameplaySystem.playerManager.player.gameObject.SetActive(false);
        GameplaySystem.playerManager.player.character.gameObject.SetActive(false);

        m_groundSlamTimelineCall.Play();
        yield return ReturnGrabTentacleToWaitForInput();

        while (isAttackDone == false)
            yield return null;

        GameplaySystem.playerManager.player.gameObject.SetActive(true);
        GameplaySystem.playerManager.player.character.gameObject.SetActive(true);
    }

    private IEnumerator HardcodedGroundSlamSequence()
    {
        GameplaySystem.playerManager.player.transform.SetParent(m_tentacleGrabHand);
        GameplaySystem.playerManager.player.gameObject.SetActive(false);
        GameplaySystem.playerManager.player.character.gameObject.SetActive(false);
        m_dummyPlayer.SetActive(true);

        m_animation.SetAnimation(0, m_grabbingAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_grabbingAnimation);
        m_animation.SetAnimation(0, m_groundSlamAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_groundSlamAnimation);
        GameplaySystem.playerManager.player.SetPosition(m_tentacleGrabHand.position);
        yield return GroundSlamRetract();
    }

    public void DamagePlayer(int damage)
    {
        GameplaySystem.playerManager.player.damageableModule.TakeDamage(damage, DChild.Gameplay.Combat.DamageType.Physical);
    }

    public void SetAttackDone()
    {
        isAttackDone = true;
    }

    public void GrabbedPlayer()
    {
        isPlayerGrabbed = true;
    }
}
