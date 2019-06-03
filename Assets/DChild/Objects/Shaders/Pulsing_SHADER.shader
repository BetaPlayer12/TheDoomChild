Shader "Gian/Pulse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GlowColor ("Glow Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_Frequency( "Glow Frequency", Float ) = 1.0
		_MinPulseVal( "Minimum Glow Multiplier", Range( 0, 1 ) ) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D	_MainTex;
		fixed4		_GlowColor;
		half		_Frequency;
		half		_MinPulseVal;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half posSin = 0.5 * sin( _Frequency * _Time.x ) + 0.5;
			half pulseMultiplier = posSin * ( 1 - _MinPulseVal ) + _MinPulseVal;
			o.Albedo = c.rgb * _GlowColor.rgb * pulseMultiplier;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
