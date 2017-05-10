Shader "Custom/Fog" 
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_intensity("Intensity", Range(1, 3)) = 1
		_startColor("Start Color", Vector) = (1, 1, 1, 1)
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
			uniform float _zoom;

			uniform float4 _startColor;
			uniform float4 _endColor;

			uniform float4 _spotPos;
			uniform float4  _spotDir;

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
				o.pos = UnityObjectToClipPos(i.pos);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, i.uv);
				o.color = lerp(_startColor, _endColor, _depth - o.uv.y / 5);
				return o;
			}

			float spot(output o);
			float4 gammaCorrection(float4 color);
			float boundary(float dSample);

			float4 frag(output o) : COLOR
			{
				//objects
				float4 fColor = tex2D(_MainTex, o.uv);

				float dSample = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, o.uv));
				float sampleDepth = Linear01Depth(dSample);

				float lighting = spot(o) * boundary(LinearEyeDepth(dSample));

				//lerp between objects and fog depending on 
				float4 fog = max(lighting, 1 - _depth) * o.color;

				if (sampleDepth == 1)
					return fog;

				return lerp(fColor, fog, pow(sampleDepth, _intensity)) * max(lighting, 1) + pow(lighting, 2) * float4(0.1, 0.1, 0.1, 0);
			}

			//calculate if the fragment is inside the spot
			float spot(output o)
			{
				float angle = 9;
				float falloff = 2;

				float camDist = 15;
				float scaling = 20;

				float fustrum  = pow(max(dot(normalize(o.uv - _spotPos.xy), _spotDir), 0),     angle);
				float dist = pow(min(1 - distance(_spotPos.xy, o.uv) * (1 + (_zoom - camDist) / scaling), 1), falloff);

				return fustrum * dist;
			}

			float4 gammaCorrection(float4 color)
			{
				float gamma = 0.7 + (_depth - 0.2) / 2;

				return float4(pow(color.xyz, 1.0 / gamma).xyz, 1);
			}

			float boundary(float dSample)
			{
				return clamp( (dSample - 14) / 1, 0, 1);
			}

		ENDCG
		}
	}
}