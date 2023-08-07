using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRandomizer : MonoBehaviour
{
    [SerializeField]
    private string[] outcomes = { "rock", "paper", "scissors" };
    [SerializeField]
    private float[] probabilities = { 80f, 10f, 10f };
    void Start()
    {
        Debug.Log(GenerateOutcome());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GenerateOutcome()
    {
        float rand = Random.Range(0f, 100f);
        Debug.Log(rand);
        switch (rand)
        {
            case float n when n <= probabilities[0]:
                //Debug.Log(outcomes[0]);
                return outcomes[0];
            case float n when n <= probabilities[0] + probabilities[1]:
               // Debug.Log(outcomes[1]);
                return outcomes[1];
            default:
               // Debug.Log(outcomes[2]);
                return outcomes[2];
        }
    }
}
