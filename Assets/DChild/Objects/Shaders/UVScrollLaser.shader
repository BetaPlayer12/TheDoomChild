// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Gian/UVScrollLaser" 
{
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
    	_Color ("Main Color", Color) = (1,1,1,1)
		_ScrollX ("Scroll X", Range(-50, 50)) = 1
		_ScrollY ("Scroll Y", Range(-50, 50)) = 1
		_Brightness ("Brightness", Range(0,10)) = 1
		_Alpha ("Alpha Range", Range(0,1)) = 1
	}
	SubShader 
	{
		Tags 
		{ 
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
		}

		CGPROGRAM
		//#pragma surface surf Lambert //Original
		#pragma surface surf Lambert alpha //Alpha Edit
		float _ScrollX;
		float _ScrollY;
		fixed4 _Color;
		half _Brightness;
		float _Alpha;

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			_ScrollX *= _Time;
			_ScrollY *= _Time;
			float2 newuv = IN.uv_MainTex + float2(_ScrollX, _ScrollY);
			o.Emission = (tex2D (_MainTex, newuv) * _Color) * _Brightness;
			o.Alpha = (tex2D (_MainTex, newuv).a * _Alpha); //Alpha Edit
		}
		ENDCG
	}
	FallBack "Diffuse"
}