using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Linq;
using Holysoft.Event;
using System;

namespace DChild.Gameplay.Characters.Enemies
{
    public class KingPusBlobDropper : MonoBehaviour
    {
        [SerializeField, TabGroup("Reference")]
        private KingPusAI m_kingPus;
        [SerializeField, TabGroup("SpawnPoints")]
        private List<Transform> m_pusSpawnPoint;
        [SerializeField, TabGroup("Blobs")]
        private List<GameObject> m_blob;

        private bool m_hasPhaseChanged;
        private bool m_hasDroppedPusBlobs;
        private int m_bodySlamCounter;
        private float m_timeLapsed;

        private void InitializePusPlacement()
        {
            for(int x=0;x<m_pusSpawnPoint.Count;x++)
            {                                           //This can be changed to make from the poolable objects
                if (!m_blob[x].activeSelf)
                {
                    m_blob[x].transform.position = m_pusSpawnPoint[x].transform.position;
                    m_blob[x].GetComponent<KingPusBlobAI>().SetMaster(m_kingPus.transform);
                    m_blob[x].GetComponent<KingPusBlobAI>().ResetPosition(m_pusSpawnPoint[x].transform.position);
                    m_blob[x].GetComponent<KingPusBlobAI>().StayDormant(UnityEngine.Random.Range(0.75f,1.25f));
                }
            }
        }
        
        private void DropPusBlob()
        {
            List<int> NumbersPicked = new List<int>();
            int BlobsToDrop = m_hasPhaseChanged ? 4 : 3;
            for(int droppedBlobs=0;droppedBlobs<BlobsToDrop;droppedBlobs++)
            {
                int x = UnityEngine.Random.Range(0, m_pusSpawnPoint.Count);
                while (NumbersPicked.Contains(x))
                {
                    x = UnityEngine.Random.Range(0, m_pusSpawnPoint.Count);
                }
                m_blob[x].GetComponent<KingPusBlobAI>().StartFall();
                NumbersPicked.Add(x);
            }
        }

        private void KingPusHasBodyslam(object sender, EventActionArgs eventArgs)
        {
            m_bodySlamCounter++;
            for (int x = 0; x < m_pusSpawnPoint.Count; x++)
            {
                if (m_blob[x].activeSelf)
                {
                    StartCoroutine(m_blob[x].GetComponent<KingPusBlobAI>().ShakeAnimRoutine());
                }
            }
            if (m_bodySlamCounter >= 3)
            {
                DropPusBlob();
                m_hasDroppedPusBlobs = true;
                m_timeLapsed = 0f;
                m_bodySlamCounter = 0;
            }
        }

        
        private void Awake()
        {
            m_kingPus.BodySlamDone += KingPusHasBodyslam;
            m_kingPus.WreckingBallDone += KingPusHasDoneWreackingBall;
            m_kingPus.PhaseChangeDone += KingPusHasChangedPhase;
        }

        private void KingPusHasDoneWreackingBall(object sender, EventActionArgs eventArgs)
        {
            DropPusBlob();
            m_hasDroppedPusBlobs = true;
            m_timeLapsed = 0f;
            m_bodySlamCounter = 0;
        }

        private void KingPusHasChangedPhase(object sender, EventActionArgs eventArgs)
        {
            m_hasPhaseChanged = true;
        }

        private void Start()
        {
            InitializePusPlacement();
        }// Rats

        private void Update()
        {
            if(m_hasDroppedPusBlobs&& m_timeLapsed < 20f)
            {
                m_timeLapsed += GameplaySystem.time.deltaTime;
                if(m_timeLapsed > 4f&&m_timeLapsed <10f)
                {
                    InitializePusPlacement();
                }
            }else if(m_timeLapsed>20f)
            {
                m_hasDroppedPusBlobs = false;
            }
        }

    }
}

