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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var down = inputState.GetButtonValue(inputButtons[0]);
            var downhHold = inputState.GetButtonHoldTime(inputButtons[0]);
            var jump = inputState.GetButtonValue(inputButtons[1]);

            if (collision.gameObject.tag == "Droppable")
            {
                Debug.Log("Test");
            }

            if (collision.gameObject.tag == "Droppable" && down && jump && downhHold < 0.1f)
            {
                body2d.velocity = Vector2.zero;
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
