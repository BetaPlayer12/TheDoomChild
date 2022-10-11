using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SampleTentacleGroundStabAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private Transform m_target;

        [SerializeField]
        private Transform m_endOfTentacle;
        public IEnumerator ExecuteAttack()
        {
            //Animation 
            Debug.Log("Execute Tentacle Stab");
            transform.Translate(Vector2.down * 1f * GameplaySystem.time.deltaTime);

            //Activate colliders and stuff


            yield return new WaitForSeconds(2f);
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //StartCoroutine(ExecuteAttack());
        }
    }
}

