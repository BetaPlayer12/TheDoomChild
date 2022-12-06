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

    [SerializeField, BoxGroup("Mouth Blast I Stuff")]
    private GameObject m_mouthBlastOneLaser;
    [SerializeField, BoxGroup("Mouth Blast I Stuff")]
    private Transform m_mouthBlastLeftSide;
    [SerializeField, BoxGroup("Mouth Blast I Stuff")]
    private Transform m_mouthBlastRightSide;
    [SerializeField, BoxGroup("Mouth Blast I Stuff")]
    private float m_mouthBlastMoveSpeed;
    [SerializeField, BoxGroup("Mouth Blast I Stuff")]
    private Vector2 m_mouthBlastOriginalPosition;
    [SerializeField, BoxGroup("Mouth Blast I Stuff")]
    private BlackBloodFlood m_blackBloodFlood;
    private bool m_doMouthBlastIAttack;
    private bool m_moveMouth;
    private int m_SideToStart;

    //Monolith Slam stuff
    [SerializeField, BoxGroup("Monolith Slam Stuff")]
    private int m_numOfMonoliths;
    [SerializeField, BoxGroup("Monolith Slam Stuff")]
    private int m_monolithCounter;
    [SerializeField, BoxGroup("Monolith Slam Stuff")]
    private bool m_triggerMonolithSlamAttack;
    [SerializeField, BoxGroup("Monolith Slam Stuff")]
    private float m_monolithTimer;
    private float m_monolithTimerValue;

    private TheOneThirdFormAI m_targetInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Attacks
    public IEnumerator TentacleCeilingAttack()
    {
        StartCoroutine(m_tentacleCeilingAttack.ExecuteAttack());
        yield return null;
    }

    public IEnumerator TentacleGrabGroundSlam()
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
}
