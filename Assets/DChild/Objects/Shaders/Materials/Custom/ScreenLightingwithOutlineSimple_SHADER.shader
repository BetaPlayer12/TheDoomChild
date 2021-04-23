// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Image Effects Simple" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SaturationAmount ("Saturation Amount", Range(0.0, 1.0)) = 1.0
		_BrightnessAmount ("Brightness Amount", Range(0.0, 1.0)) = 1.0
		_ContrastAmount ("Contrast Amount", Range(0.0,1.0)) = 1.0

		//Simple Shader
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline Width", Range (-10, 10)) = .005
		//Simple Shader
	}
	SubShader 
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float _SaturationAmount;
			uniform float _BrightnessAmount;
			uniform float _ContrastAmount;
			
			float3 ContrastSaturationBrightness( float3 color, float brt, float sat, float con)
			{
				//RGB Color Channels
				float AvgLumR = 0.5;
				float AvgLumG = 0.5;
				float AvgLumB = 0.5;
				
				//Luminace Coefficients for brightness of image
				float3 LuminaceCoeff = float3(0.2125,0.7154,0.0721);
				
				//Brigntess calculations
				float3 AvgLumin = float3(AvgLumR,AvgLumG,AvgLumB);
				float3 brtColor = color * brt;
				float intensityf = dot(brtColor, LuminaceCoeff);
				float3 intensity = float3(intensityf, intensityf, intensityf);
				
				//Saturation calculation
				float3 satColor = lerp(intensity, brtColor, sat);
				
				//Contrast calculations
				float3 conColor = lerp(AvgLumin, satColor, con);
				
				return conColor;
				
			}
			
			
			float4 frag (v2f_img i) : COLOR
			{
				float4 renderTex = tex2D(_MainTex, i.uv);
				
				
				renderTex.rgb = ContrastSaturationBrightness(renderTex.rgb, _BrightnessAmount, _SaturationAmount, _ContrastAmount); 
				
				return renderTex;
			
			}
			
			ENDCG
		}

		Pass
		{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

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

			/*void vert (inout appdata_full v) {
				v.vertex.xyz += v.normal * _Outline;
			}*/

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				v.vertex.xyz += v.normal * _Outline;
				o = v;
				return v;
			}
			ENDCG

			ZWrite on
			//Simple Shader
		}
	}
}﻿