using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class PlotformDrop : PlayerBehaviour
    {
        [SerializeField]
        private CapsuleCollider2D collider2D;
        private Collider2D gameObjectCollider;
        public float enableCollider = 0.3f;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var down = inputState.GetButtonValue(inputButtons[0]);
            var downhHold = inputState.GetButtonHoldTime(inputButtons[0]);
            var jump = inputState.GetButtonValue(inputButtons[1]);
          
            if (collision.gameObject.tag == "Droppable" && down && jump && downhHold < 0.5)
            {
                Debug.Log("test");
                rigidBody.velocity = Vector2.zero;
                collider2D.isTrigger = true;
                StartCoroutine(EnableColliderRoutine());
            }
        }

      


        IEnumerator EnableColliderRoutine()
        {
            yield return new WaitForSeconds(enableCollider);
            
            collider2D.isTrigger = false;
        }
    }

}
