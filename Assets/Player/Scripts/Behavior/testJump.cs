using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class testJump : MonoBehaviour
    {
        protected float Animation;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Animation += Time.deltaTime;
            Animation = Animation % 1f;

            Debug.Log(Vector3.forward);
            transform.position = MathParabola.Parabola(Vector2.zero, new Vector2(20, 0), 15.0f, Animation / 1f);



            //Debug.Log(transform.position);
            //Debug.Log(new Vector2(transform.position.x, transform.position.y));
            ////transform.position = MathParabola.Parabola(transform.position, new Vector2(transform.position.x + 20, 0), 15.0f, Animation / 1f);


        }
    }

}