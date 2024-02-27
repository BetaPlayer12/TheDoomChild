using DChild;
using DChild.Gameplay.Characters.Enemies;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSporeBulbHandle : MonoBehaviour
{
    [SerializeField]
    private MotherMantisAI m_mothermantisAI;
    [SerializeField]
    private List<Transform> m_patterns;
    [SerializeField]
    private List<Transform> m_pattern1;
    [SerializeField]
    private List<Transform> m_pattern2;
    [SerializeField]
    private GameObject m_sporeProjectile;
    [SerializeField]
    private List<GameObject> m_bulbList;

    private bool leftHandRaised;
    private bool rightHandRaised;
    private bool bothHandRaised;

    private void OnRightHandRaised(object sender, EventActionArgs eventActionArgs)
    {
        leftHandRaised = false;
        rightHandRaised = true;
        bothHandRaised = false;
        StartCoroutine(SpawnSeedsRoutine(m_pattern2));
    }
    private void OnLefttHandRaised(object sender, EventActionArgs eventActionArgs)
    {
        leftHandRaised = true;
        rightHandRaised = false;
        bothHandRaised = false;
        StartCoroutine(SpawnSeedsRoutine(m_pattern1));
    }
    private void OnBothHandRaised(object sender, EventActionArgs eventActionArgs)
    {
        leftHandRaised = false;
        rightHandRaised = false;
        bothHandRaised = true;
        StartCoroutine(SpawnSeedsRoutine(m_pattern1));
    }

    private IEnumerator SpawnSeedsRoutine(List<Transform> spawnpoint)
    {
        yield return new WaitForSeconds(0.8f);
        for (int x = 0; x < spawnpoint.Count; x++)
        {
            var temp = this.InstantiateToScene(m_sporeProjectile, new Vector2(spawnpoint[x].position.x, spawnpoint[x].position.y), Quaternion.identity);
            m_bulbList.Add(temp);
        }
        yield return new WaitForSeconds(1.5f);
        if (leftHandRaised || rightHandRaised)
        {
            Detonate();
        }
        if (bothHandRaised)
        {
            StartCoroutine(DetonateAll());
        }
        yield return null;
    }
    private IEnumerator DetonateAll()
    {
        for (int x = 0; x < m_bulbList.Count; x++)
        {
            m_bulbList[x].GetComponent<FlowerSporeProjectile>().Detonate();
        }
        m_bulbList.Clear();
        yield return null;
    }
    private void Detonate()
    {
        for (int x = 0; x < m_bulbList.Count; x++)
        {
            m_bulbList[x].GetComponent<FlowerSporeProjectile>().Detonate();
        }
        m_bulbList.Clear();
    }

    private
    void Start()
    {
        m_mothermantisAI.OnHandRaisedLeft += OnLefttHandRaised;
        m_mothermantisAI.OnHandRaisedRight += OnRightHandRaised;
        m_mothermantisAI.OnBothHandRaised += OnBothHandRaised;
    }

    private void OnDestroy()
    {
        m_mothermantisAI.OnHandRaisedLeft -= OnLefttHandRaised;
        m_mothermantisAI.OnHandRaisedRight -= OnRightHandRaised;
        m_mothermantisAI.OnBothHandRaised -= OnBothHandRaised;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
