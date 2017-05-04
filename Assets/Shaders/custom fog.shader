Shader "Custom/Fog" 
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_intensity("Intensity", Range(1, 3)) = 1
		_startColor("Start Color", Vector) = (1, 1, 1, 1)
		_depth("Depth", Range(0, 1)) = 1
	}
	
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthTexture;
			uniform float _intensity;

			uniform float _depth;

			uniform float4 _startColor;
			uniform float4 _endColor;


			struct input
			{
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct output
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;

				float4 color : COLOR0;
			};

			output vert(input i) 
			{
				output o;
				o.pos = mul(UNITY_MATRIX_MVP, i.pos);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, i.uv);
				o.color = lerp(_startColor, _endColor, _depth + o.uv.y / 5);
				return o;
			}

			float4 frag(output o) : COLOR
			{
				float4 fColor = tex2D(_MainTex, o.uv);
				float  distance = pow(Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, half2(o.uv.x, 1 - o.uv.y)))), _intensity);
				
				return lerp(fColor, o.color, distance);
			}
		ENDCG
		}
	}
}