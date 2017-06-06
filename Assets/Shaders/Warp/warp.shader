Shader "custom/warp"
{
	Properties{
		_warp ("warp amount", Range(0,0.1)) = 0.01
		_speed ("speed", Range(0,25)) = 15
		_mask ("edge mask", Range(0,25)) = 20
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
			uniform float _warp;
			uniform float _speed;
			uniform float _mask;

			fixed4 frag (v2f i) : SV_Target
			{
				float time = _Time;
				float warp = sin(time * _speed + i.uv * 8 + i.uv.y) * _warp;

				float edges = abs(i.uv.x * 2 - 1);
				float edgeMask = 1 - pow(edges, _mask);

				i.uv += float2(warp * edgeMask, 0);
				fixed4 scene = tex2D(_cleanScene, i.uv);

				return scene;
			}
			ENDCG
		}
	}
}