Shader "Custom/Specular Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _EmissiveTex ("Emission", 2D) = "white" {}
        _EmissionStrength ("Emission Strength", Range (-10, 10)) = 1
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        [MaterialToggle] OutlineToggle ("Enable Outline", Float) = 0
        [MaterialToggle] EmissionToggle ("Enable Emission", Float) = 0

        //Advance Shader
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline Width", Range (-10, 10)) = .005
        _OutlineColor2 ("Outline Color 2", Color) = (0,0,0,1)
        _Outline2 ("Outline Width 2", Range (-10, 10)) = .005
        //Advance Shader
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #pragma multi_compile _ EMISSIONTOGGLE_ON

        sampler2D _MainTex;
        sampler2D _EmissiveTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissiveTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _EmissionStrength;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            #if defined(EMISSIONTOGGLE_ON)
            fixed4 d = tex2D (_EmissiveTex, IN.uv_EmissiveTex) * _Color * _EmissionStrength;
            o.Emission = d.rgb;
            #endif
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
        
        Pass {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ OUTLINETOGGLE_ON

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

            float _Outline;
            float4 _OutlineColor;

            v2f vert(appdata v) {
                v2f o;
                
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 norm = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(norm.xy);

                float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;

                #if defined(OUTLINETOGGLE_ON)
                o.pos.xy += offset * o.pos.z * _Outline;
                o.color = _OutlineColor;
                #else
                o.pos.xy += offset * o.pos.z * 0;
                o.color = 0;
                #endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }

        Pass {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ OUTLINETOGGLE_ON

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

            float _Outline2;
            float4 _OutlineColor2;

            v2f vert(appdata v) {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                float3 norm = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(norm.xy);

                float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;

                #if defined(OUTLINETOGGLE_ON)
                o.pos.xy += offset * o.pos.z * _Outline2;
                o.color = _OutlineColor2;
                #else
                o.pos.xy += offset * o.pos.z * 0;
                o.color = 0;
                #endif

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
