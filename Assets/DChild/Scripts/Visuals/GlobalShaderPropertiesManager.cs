using UnityEngine;

namespace DChild.Visuals
{
    public class GlobalShaderPropertiesManager : MonoBehaviour
    {
        private void Start()
        {
            Shader.SetGlobalInteger("_IsApplicationPlaying", 1);
        }

        private void Update()
        {
            var time = Time.unscaledTime;
            Shader.SetGlobalFloat("_UnscaledTime", time);
            Shader.SetGlobalFloat("_UnscaledSineTime", Mathf.Sin(time));
            Shader.SetGlobalFloat("_UnscaledCosineTime", Mathf.Cos(time));
            Shader.SetGlobalFloat("_UnscaledDeltaTime", Time.unscaledDeltaTime);
        }

        private void OnDestroy()
        {
            Shader.SetGlobalInteger("_IsApplicationPlaying", 0);
        }
    }

}