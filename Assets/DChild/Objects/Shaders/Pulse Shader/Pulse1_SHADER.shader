// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Alan Zucconi/Tentacle" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_PulseAmount ("Pulse amount", Range(0,100)) = 0.1 // metres
		_PulsePeriod ("Pulse period", Float) = 1          // seconds
		_PulseTex ("Pulse tex", 2D) = "white" {}

		//Vertex Shading Example
		_Value1 ("Value 1", Float) = 1
		_Value2 ("Value 2", Float) = 1
		_Value3 ("Value 3", Float) = 1
		
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

		}
	SubShader {

        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

    	LOD 200
	    Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
		
		
		CGPROGRAM
		/*// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0*/
		//#pragma target 3.0
		//#pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow
        #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #pragma multi_compile _ GRASSANIMATION_ON //Grass Animation Toggle
        #include "UnitySprites.cginc"

		/*sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _Occlusion;*/

		float _PulseAmount;
		float _PulsePeriod;
		sampler2D _PulseTex;

		float _Value1;
		float _Value2;
		float _Value3;

		struct Input {
		    float2 uv_MainTex;
		    fixed4 color;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		/*void vert (inout appdata_full v) {
		    v.vertex.xyz += v.normal * 1;
		}*/

		/*void vert (inout appdata_full v, out Input o) {
			// Position component
    		float y = v.texcoord.xy + 0.5;

		    // Effect multiplier
			fixed4 c = tex2Dlod (_PulseTex, float4(v.texcoord.xy,0,0));
			float pulse = c.b; // Uses the blue channel

			// Time and position component
			const float TAU = 6.28318530718;
			float time = (sin (_Time.y / _PulsePeriod * TAU) + 1.0) / 2.0; // [0,1]

			v.vertex.xy += v.normal * pulse * time * _PulseAmount * y;

            v.vertex = UnityFlipSprite(v.vertex, _Flip);

            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap (v.vertex);
            #endif

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color;
		}*/

		//Vertex function
		fragment vert (vertexInput i) 
		{
			fragmentInput o;

			//Fat Mesh
			//i.vertex.xyz += i.normal * _Value1;

			//Waving Mesh
			i.vertex.x += sin((i.veertex.y + _Time * _Value3 ) * _Value2) * _Value1;

			//Bubbling mesh
			//i.vertex.xyz += i.normal * (sin((i.vertex.x + _Time * _Value3) * _Value2) + cos((i.vertex.z + _Time * _Value3) * _Value2)) * _Value1;

			//Color
			//o.color = i.texcoord;
			o.color = float4( i.normal, 1) * 0.5 + 0.5;

			//This line must be after the vertex manipulation
			o.pos = UnityObjectToClipPos(i.vertex);
			return o;
		}

		//Fragment function
		float4 frag( fragmentInput i) : Color
		{
			return i.color;
		}

		//Original
		/*void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			
			o.Occlusion = tex2D (_Occlusion, IN.uv_MainTex).r;
			o.Normal = UnpackNormal(tex2D (_Normal, IN.uv_MainTex));
		}*/
		//2D

		void surf (Input IN, inout SurfaceOutput o) 
		{
		    //fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		    /*fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb * c.a;
		    o.Alpha = c.a;*/

            fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb * c.a;
            o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}