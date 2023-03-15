using UnityEngine;

namespace DChild.Visuals
{
    public class GlobalShaderPropertiesManager : MonoBehaviour
    {
        private static GlobalShaderPropertiesManager instance;

        private void Start()
        {
            if (instance == null)
            {
                Shader.SetGlobalInteger("_IsApplicationPlaying", 1);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
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
            if (this == instance)
            {
                Shader.SetGlobalInteger("_IsApplicationPlaying", 0);
                instance = null;
            }
        }
    }

}