Shader "custom/fx_vignette"
{
	Properties {
		_screenWarp("screen warp", 2D) = ""{}
		_int("intensity", float) = 1
		_spd("speed", float) = 0

	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }

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

			uniform sampler2D _scene;
			uniform sampler2D _screenWarp;

			uniform float _range;
			uniform float _width;
			uniform float _intensity;

			uniform float _int;
			uniform float _spd;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float time = _Time * _spd;
				
				half4 normalCol1 = tex2D(_screenWarp, i.uv + float2(0.5, time));
				half4 normalCol2 = tex2D(_screenWarp, i.uv + float2(time * 0.75, 0.25));
				float warp = (normalCol1.g) + (normalCol2.g);
				warp = warp * 0.5 - 0.25;
				warp *= _int;

				float dist = distance(i.uv, float2(0.5,0.5));
				float start = _range - _width;

				float diff = saturate((dist - start) / (_range - start));
				diff = min(diff, _intensity);

				half4 scene = tex2D(_scene, i.uv + warp * diff);
				half4 vignette = half4(1,1,1,1);

				float2 centerShift = i.uv - float2(0.5, 0.5);
				i.uv += (centerShift * warp) * diff;

				half4 centerScene = tex2D(_scene, i.uv);

				return centerScene + vignette*diff;
//				return scene + vignette * diff;
			}
			ENDCG
		}
	}
}
