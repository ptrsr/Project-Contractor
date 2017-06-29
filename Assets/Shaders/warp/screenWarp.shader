Shader "custom/fx_screenWarp"
{
	Properties{
		_warpNormal ("warp normal", 2D) = ""{}

		_warpN ("warp normal", Range(0,0.1)) = 0
		_speedN ("speed normal", Range(0,20)) = 0
		_warpS ("warp side", Range(0,0.1)) = 0
		_speedS ("speed side", Range(0,20)) = 0

		_mask ("edge mask", Range(0,25)) = 0
	}
	
	SubShader
	{

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			uniform sampler2D _cleanScene;
			uniform sampler2D _warpNormal;
			uniform float _warpN;
			uniform float _speedN;
			uniform float _warpS;
			uniform float _speedS;
			uniform float _mask;

			fixed4 frag (v2f i) : SV_Target
			{	
				float time = _Time;
				float shift = time;

				float edges = abs(i.uv.x * 2 - 1);
				float edgeMask = 1 - pow(edges, _mask);

				float3 warpNormal = UnpackNormal(tex2D(_warpNormal, i.uv + float2(time * _speedN ,0) )) * 0.1;
				float warpSideShift = sin(time * _speedS + i.uv * 8) * _warpS;

				float2 uvNormal = warpNormal.xz * _warpN;
				float2 uvSideShift = float2(warpSideShift * edgeMask, 0);
				float2 uvFinal = i.uv + uvNormal + uvSideShift;

				fixed4 scene = tex2D(_cleanScene, uvFinal);
				return scene;
			}
			ENDCG
		}
	}
}