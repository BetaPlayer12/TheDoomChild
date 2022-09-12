using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class PositionClamper : MonoBehaviour
    {
        public enum Options { Clamp, useRelativeDistance }

        [System.Serializable]
        private struct Handle
        {
            
            [SerializeField, EnumToggleButtons]
            private Options m_options;
            [SerializeField, ShowIf("m_options", Options.Clamp), HorizontalGroup("Number")]
            private float m_min;
            [SerializeField, ShowIf("m_options", Options.Clamp), HorizontalGroup("Number")]
            private float m_max;

            [SerializeField]
            private bool m_willClamp;



           

            //[SerializeField]
            //private bool m_useRelativeDistance;
            [SerializeField, ShowIf("m_options", Options.useRelativeDistance), HorizontalGroup("RelativeDistance")]
            private float m_minRelativeDistance;
            [SerializeField, ShowIf("m_options", Options.useRelativeDistance), HorizontalGroup("RelativeDistnce")]
            private float m_maxRelativeDistance;

            public bool willClamp => m_willClamp;
            public float min => m_min;
            public float max => m_max;

            //public bool useRelativeDistance => m_useRelativeDistance;
            public float minRelativeDistance => m_minRelativeDistance;
            public float maxRelativeDistance => m_maxRelativeDistance;

            public Options clampOptions => m_options;
        }

        [SerializeField]
        private Handle m_clampX;
        [SerializeField]
        private Handle m_clampY;


        [SerializeField]
        private bool m_referenceRigidBody2D;
        [SerializeField, ShowIf("m_referenceRigidBody2D"), HorizontalGroup("RigidBody")]
        private Rigidbody2D m_rigidbody2D;


        private float GetClampValue(Handle handle, float value)
        {

            if (handle.willClamp)
            {

                if (handle.clampOptions == Options.Clamp)
                {
                    if (m_referenceRigidBody2D)
                    {
                        if (m_rigidbody2D.IsSleeping())
                        {
                            //Debug.Log("Is Sleeping");
                        }
                        else
                        {
                            //Debug.Log("IsClamping");
                            return Mathf.Clamp(value, handle.min, handle.max);
                        }
                    }
                    else
                    {

                        //Debug.Log("Went In");
                        return Mathf.Clamp(value, handle.min, handle.max);
                    }


                }
                if (handle.clampOptions == Options.useRelativeDistance)
                {
                    //Debug.Log("Relative went in");
                    var startPos = value;
                    var tempClamp = Mathf.Clamp(value, handle.minRelativeDistance, handle.maxRelativeDistance);
                    return Mathf.Clamp(value, startPos, tempClamp);

                }  
            }
            return value;

        }

        private void LateUpdate()
        {

            var position = transform.position;

            position.x = GetClampValue(m_clampX, position.x);
            position.y = GetClampValue(m_clampY, position.y);
            transform.position = position;






        }

    }
}