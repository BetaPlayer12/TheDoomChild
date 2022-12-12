using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using UnityEngine;

public class TheOneThirdFormAttacks : MonoBehaviour
{
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private TentacleGroundStabAttack m_tentacleGroundStabAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private TentacleCeilingAttack m_tentacleCeilingAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private MovingTentacleGroundAttack m_movingTentacleGroundAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private ChasingGroundTentacleAttack m_chasingGroundTentacleAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private MouthBlastIIAttack m_mouthBlastIIAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private SlidingStoneWallAttack m_slidingWallAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private MonolithSlamAttack m_monolithSlamAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private TentacleBlastAttack m_tentacleBlastAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private TentacleGrabAttack m_tentacleGrabScriptedAttack;
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private BubbleImprisonmentAttack m_bubbleImprisonmentAttack;

    private TheOneThirdFormAI m_targetInfo;

    //Attacks
    public IEnumerator TentacleCeilingAttack()
    {
        StartCoroutine(m_tentacleCeilingAttack.ExecuteAttack());
        yield return null;
    }

    public IEnumerator TentacleGrab()
    {
        StartCoroutine(m_tentacleGrabScriptedAttack.ExecuteAttack());
        yield return null;
    }

    public IEnumerator MouthBlastWall()
    {
        StartCoroutine(m_mouthBlastIIAttack.ExecuteAttack());
        yield return null;
    }

    public IEnumerator TentacleGroundStab(AITargetInfo Target)
    {
        StartCoroutine(m_tentacleGroundStabAttack.ExecuteAttack(Target));
        yield return null;
    }

    public IEnumerator MovingTentacleGround()
    {
        StartCoroutine(m_movingTentacleGroundAttack.ExecuteAttack());
        yield return null;
    }

    public IEnumerator ChasingGroundTentacle()
    {
        StartCoroutine(m_chasingGroundTentacleAttack.ExecuteAttack());
        yield return null;
    }

    public IEnumerator MonolithSlam(AITargetInfo Target)
    {
        StartCoroutine(m_monolithSlamAttack.ExecuteAttack(Target));
        yield return null;
    }

    public IEnumerator TentacleBlast()
    {
        StartCoroutine(m_tentacleBlastAttack.ExecuteAttack());
        yield return null;
    }

    public IEnumerator SlidingStoneWallAttack(AITargetInfo Target)
    {
        StartCoroutine(m_slidingWallAttack.ExecuteAttack(Target));
        yield return null;
    }

    public IEnumerator BubbleImprisonment(AITargetInfo Target)
    {
        StartCoroutine(m_bubbleImprisonmentAttack.ExecuteAttack(Target));
        yield return null;
    }
}
