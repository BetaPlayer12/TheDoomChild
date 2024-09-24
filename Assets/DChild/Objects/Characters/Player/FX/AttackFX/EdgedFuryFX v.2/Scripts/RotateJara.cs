using UnityEngine;

namespace MyNamespace
{
    public class RotateJara : MonoBehaviour
    {
        public GameObject gameObject;
        public Vector3 rotationVector;
        public float xAngle;
        public float yAngle;
        public float zAngle;
        public Space space;
        public bool perSecond;
        public bool everyFrame;
        public bool lateUpdate;
        public bool fixedUpdate;

        private void Start()
        {
            if (!everyFrame && !lateUpdate && !fixedUpdate)
            {
                DoRotate();
                enabled = false;
            }
        }

        private void Update()
        {
            if (!lateUpdate && !fixedUpdate)
            {
                DoRotate();
            }
        }

        private void LateUpdate()
        {
            if (lateUpdate)
            {
                DoRotate();
            }

            if (!everyFrame)
            {
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (fixedUpdate)
            {
                DoRotate();
            }

            if (!everyFrame)
            {
                enabled = false;
            }
        }

        private void DoRotate()
        {
            if (gameObject == null)
            {
                return;
            }

            // Use rotationVector if specified
            var rotate = rotationVector == Vector3.zero ? new Vector3(xAngle, yAngle, zAngle) : rotationVector;

            // Override any axis
            if (!Mathf.Approximately(xAngle, 0f)) rotate.x = xAngle;
            if (!Mathf.Approximately(yAngle, 0f)) rotate.y = yAngle;
            if (!Mathf.Approximately(zAngle, 0f)) rotate.z = zAngle;

            // Apply rotation
            if (!perSecond)
            {
                gameObject.transform.Rotate(rotate, space);
            }
            else
            {
                gameObject.transform.Rotate(rotate * Time.deltaTime, space);
            }
        }
    }
}
