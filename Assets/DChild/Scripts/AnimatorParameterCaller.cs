using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    public class AnimatorParameterCaller : MonoBehaviour
    {
        [SerializeField,InfoBox("Always Call SetTargetParameter First before calling SetValue")]
        private Animator m_animator;
        private string m_parameterName;

        public void SetTargetParameter(string parameter) => m_parameterName = parameter;

        public void SetValue(int value) => m_animator.SetInteger(m_parameterName, value);
        public void SetValue(float value) => m_animator.SetFloat(m_parameterName, value);
        public void SetValue(bool value) => m_animator.SetBool(m_parameterName, value);
    }
}