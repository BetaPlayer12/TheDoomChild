using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheOneThirdFormAttacks : MonoBehaviour
{
    [SerializeField, BoxGroup("The One Third Form Attacks")]
    private TentacleGroundStabAttack m_tentacleStabAttack;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Attacks
    [Button]
    public void TentacleCeilingAttack()
    {
        StartCoroutine(m_tentacleCeilingAttack.ExecuteAttack());
    }

    [Button]
    public void TentacleGrabGroundSlam()
    {
        StartCoroutine(m_tentacleGrabScriptedAttack.ExecuteAttack());
    }

    [Button]
    public void MouthBlastWall()
    {
        StartCoroutine(m_mouthBlastIIAttack.ExecuteAttack());
    }
}
