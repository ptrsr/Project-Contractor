Shader "custom/fx_vignette"
{
	Properties {
		_screenWarp("screen warp", 2D) = ""{}
		_int("intensity", float) = 1
		_spd("speed", float) = 0
		_push("push", Range(0,0.5)) = 0
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
			uniform float _curve;

			uniform float _int;
			uniform float _spd;
			uniform float _push;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			//–––––––––––––––––––––––––––––––––––––
			// IN PROGRESS — DO NOT USE
			float radialSampling (float2 uv) {
				float2 dir = uv - float2(.5, .5);
				float2 source = float2(0,1);
				float deg = dot(dir, source) + 1;
				deg *= 0.25;

				if (uv.x < 0.5)
					deg += 0.5;

				return deg;
			}
			// IN PROGRESS — DO NOT USE
			//–––––––––––––––––––––––––––––––––––––
			// IN PROGRESS — DO NOT USE
			float screenEdgeMasking (float2 uv) {
				uv -= 0.5;
				uv = abs(uv * 2);
				uv = pow(uv, 5);
				return uv;
			}
			// IN PROGRESS — DO NOT USE
			//–––––––––––––––––––––––––––––––––––––


			fixed4 frag (v2f i) : SV_Target
			{
				float time = _Time * _spd;

				//–––––––––––––––––––––––––––––––––––––
				// IN PROGRESS — DO NOT USE
				// float radial = radialSampling(i.uv);
				// float warpRadial = tex2D(_screenWarp, float2(radial, 0.5));
				//–––––––––––––––––––––––––––––––––––––

				half4 normalCol1 = tex2D(_screenWarp, i.uv + float2(0.5, time));
				half4 normalCol2 = tex2D(_screenWarp, i.uv + float2(time * 0.75, 0.25));
				float warp = (normalCol1.g) + (normalCol2.g);
				warp = warp * 0.5 - 0.25;
				warp *= _int;

				float dist = distance(i.uv, float2(0.5,0.5));
				float start = _range - _width;

				// DIFF FOR VIGNETTE
				float diff = saturate((dist - start) / (_range - start));
				diff = pow(diff, 1 + _curve);
				diff = min(diff, _intensity);

				// DIFF FOR WARPING
				float warpdiff = saturate((dist - start) / ((_range - 0.2) - start));
				warpdiff = pow(diff, 1 + _curve);
				warpdiff = min(diff, _intensity);

				// DISTORTION
				float2 centerShift = i.uv - float2(0.5, 0.5);
				float2 uvWarp = i.uv + (centerShift * warp) * warpdiff;


				float2 uvCenter = i.uv * 2 - 1;

				float2 dir = normalize(uvCenter);

				float2 uvPush = i.uv + (dir * _push) * (1-diff);

				half4 pushScene = tex2D(_scene, uvPush);

				half4 warpScene = tex2D(_scene, uvWarp);
				half4 vignette = half4(1,1,1,1);
				vignette *= diff;

//				return pushScene + vignette;
				return warpScene + vignette;
			}
			ENDCG
		}
	}
}
