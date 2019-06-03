// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Zky/Vegetation/WindShaderV2"
{
    Properties
    {
        _MainTex("_MainTex", 2D) = "white" {}
        _alpha_threshold("_alpha_threshold", Range(0.01,0.99) ) = 0.3
           
        _windFreq("Wind Frequency", Float) = 30
        _windScale("Wind Scale / Ausschlag", Float) = 0.25
        _windAmplitude("Wind Amplitude / Versatz", Float) = 500
        }
     
    SubShader
    {
        Tags
        {
            "Queue"="Geometry"
            "IgnoreProjector"="True"
            "RenderType"="TransparentCutout"
        }
 
         
        //ShaderLab Presets
        Cull Off
        ZWrite On
        ZTest LEqual
        ColorMask RGBA
 
     
       //MainShader
        CGPROGRAM
        //#pragma surface surf Lambert
        #pragma surface surf Lambert  addshadow vertex:vert nolightmap noforwardadd
     
        sampler2D _MainTex;
        float _alpha_threshold;
       
        uniform float _windFreq;
        uniform float _windScale;
        uniform float _windAmplitude;
                 
        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };
     
           //Vertex Displacement (WindAnimation as Sinuswaves)
        void vert (inout appdata_full v, out Input o)
        {
            float disFreq = 0.33 * _windFreq;
            float disScale = 0.5 * _windScale;
            float disAmp = 0.033 * _windAmplitude;
           
            float2 wind = (0,0);
            float3 vWorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
 
            wind.x = sin(vWorldPos.x * disAmp + _Time * disFreq) * disScale;
            wind.y = cos(vWorldPos.z * disAmp + _Time * disFreq) * disScale;
           
            //wind.x = sin(disAmp + _Time * disFreq) * disScale;
            //wind.y = cos(disAmp + _Time * disFreq) * disScale;
           
            //Fadeoff effect to the bottom
            wind = (wind * v.vertex.y);
           
            if(v.vertex.y > 2)
            {
                   
                v.vertex.x = v.vertex.x + wind.x;
                v.vertex.z = v.vertex.z + wind.y;
            }
           
            //wind = mul(wind, UNITY_MATRIX_IT_MV).xyz;
            //wind = mul(wind, UNITY_MATRIX_MVP);
 
 
        }
        ENDCG
    }
    Fallback "Diffuse"
}