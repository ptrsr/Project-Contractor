Shader "custom/sonar"
{
	Properties
	{
		_MainTex   ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv	  : TEXCOORD0;
				float4 ray    : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex  : SV_POSITION;
				float4	color  : COLOR0;

				float2 uv	   : TEXCOORD0;
				float2 uvdepth : TEXCOORD1;
				float4 ray	   : TEXCOORD2;
			};
			
			uniform float4 _startColor;
			uniform float4 _endColor;
			uniform float  _depth;


			v2f vert (appdata v)
			{
				v2f o;

				o.vertex  = UnityObjectToClipPos(v.vertex);
				o.color   = lerp(_startColor, _endColor, _depth - v.uv.y / 5);

				o.uv	  = v.uv;
				o.uvdepth = v.uv.xy;
				o.ray	  = v.ray;
				return o;
			}

			//textures
			uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthTexture;


			//sonar
			uniform int    _maxPulses;
			uniform float  _aPulseDist[20];
			uniform float4 _aOrigin   [20];
			uniform float  _aWidth    [20];

			//fog
			uniform float _zoom;

			uniform float4 _spotPos;
			uniform float4 _spotDir;

			uniform float _intensity;

			//functions
			float spot(v2f i);
			float4 gammaCorrection(float4 color);
			float boundary(float dSample);

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 fColor = tex2D(_MainTex, i.uv);
				float dSample = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));

				float linearDepth = Linear01Depth(dSample);
				float4 dir = linearDepth * i.ray;
				float3 pos = _WorldSpaceCameraPos + dir;
				fixed4 pColor = fixed4(0,0,0,0);

				for(int j = 0; j < _maxPulses; j++) 
				{
					float dist = distance(pos, _aOrigin[j]);

					if (dist < _aPulseDist[j] && dist > _aPulseDist[j] - _aWidth[j])
					{
						float diff = 1 - (_aPulseDist[j] - dist) / (_aWidth[j]);
						pColor = lerp(fixed4(1,0,0,1), fixed4(1,1,1,1), diff);
						pColor *= diff;
					}
				}

				float lighting = spot(i) * boundary(LinearEyeDepth(dSample));
				float4 fog = max(lighting, 1 - _depth) * i.color;
				float4 volumetric = pow(lighting, 2) * float4(0.1, 0.1, 0.1, 0);

				if (linearDepth == 1)
					return fog + volumetric;

				float4 finalColor = lerp(fColor, fog, pow(linearDepth, _intensity)) * max(lighting, 1) + volumetric;

				finalColor.g += pColor.r;

				return finalColor;
			}

			float spot(v2f i)
			{
				float angle = 9;
				float falloff = 2;

				float camDist = 15;
				float scaling = 20;

				float fustrum = pow(max(dot(normalize(i.uv - _spotPos.xy), _spotDir), 0), angle);
				float dist = pow(min(1 - distance(_spotPos.xy, i.uv) * (1 + (_zoom - camDist) / scaling), 1), falloff);

				return fustrum * dist;
			}

			float4 gammaCorrection(float4 color)
			{
				float gamma = 0.7 + (_depth - 0.2) / 2;

				return float4(pow(color.xyz, 1.0 / gamma).xyz, 1);
			}

			float boundary(float dSample)
			{
				return clamp((dSample - 14) / 1, 0, 1);
			}

			ENDCG
		}
	}
}