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

    //stuff for tentacle stab attack
    [SerializeField, BoxGroup("Tentacle Stab Attack Stuff")]
    private float m_tentacleStabTimer = 0f;
    [SerializeField, BoxGroup("Tentacle Stab Attack Stuff")]
    private Transform m_tentacleStabSpawnHeight;
    private int m_tentacleStabCount = 0;
    private float m_tentacleStabTimerValue;
    private bool triggerTentacleGroundStab;

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
        //if (triggerTentacleGroundStab)
        //{
        //    m_tentacleStabTimer -= GameplaySystem.time.deltaTime;

        //    Vector2 tentacleSpawn = new Vector2(m_targetInfo.position.x, m_tentacleStabSpawnHeight.position.y);
        //    if (m_tentacleStabTimer <= 0)
        //    {
        //        m_currentAttackCoroutine = StartCoroutine(m_tentacleStabAttack.ExecuteAttack(tentacleSpawn));
        //        m_tentacleStabCount++;
        //        m_tentacleStabTimer = m_tentacleStabTimerValue;
        //    }

        //    if (m_tentacleStabCount > 4)
        //    {
        //        m_tentacleStabCount = 0;
        //        triggerTentacleGroundStab = false;
        //    }
        //}
    }

    //Attacks
    [Button]
    public IEnumerator TentacleCeilingAttack()
    {
        StartCoroutine(m_tentacleCeilingAttack.ExecuteAttack());
        yield return null;
    }

    [Button]
    public IEnumerator TentacleGrabGroundSlam()
    {
        StartCoroutine(m_tentacleGrabScriptedAttack.ExecuteAttack());
        yield return null;
    }

    [Button]
    public IEnumerator MouthBlastWall()
    {
        StartCoroutine(m_mouthBlastIIAttack.ExecuteAttack());
        yield return null;
    }

    [Button]
    public IEnumerator TentacleGroundStab(Vector2 PlayerPosition)
    {
        StartCoroutine(m_tentacleGroundStabAttack.ExecuteAttack(PlayerPosition));
        yield return null;
    }
}
