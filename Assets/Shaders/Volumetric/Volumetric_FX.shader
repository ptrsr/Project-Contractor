Shader "custom/volumetric_fx"
{
	SubShader
	{
		Pass
		{
			Cull Off ZTest On
			Blend SrcAlpha OneMinusSrcAlpha
			Tags{ "RenderType" = "Transparent" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv	  : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv	  : TEXCOORD0;
			};




			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			uniform sampler2D _LastCameraDepthTexture;
			uniform float _height;
			

			fixed4 frag (v2f i) : SV_Target
			{
				float depth = 1 - tex2D(_LastCameraDepthTexture , float2(0, i.uv.x));
				
				if (depth > i.uv.y)
					depth = 1;
				else
					depth = 0;

				float horizontal = 1 - pow(abs(i.uv.x - 0.5f) * 2, 2);
				float vertical = 1 - pow(i.uv.y, 2);


				return fixed4(1, 1, 1, depth * horizontal * vertical);
			}
			ENDCG
		}

		Pass
			{
				Cull Off ZTest On
				Blend SrcAlpha OneMinusSrcAlpha
				Tags{ "RenderType" = "Transparent" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				static const float PI = 3.14159265f;

				#include "UnityCG.cginc"

				struct appdata
			{
				float4 vertex : POSITION;
				float2 uv	  : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv	  : TEXCOORD0;
			};




			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			uniform sampler2D _LastCameraDepthTexture;
			uniform float _height;

			fixed4 frag(v2f i) : SV_Target
			{
				float uvCoord = ((1 / i.uv.x) * i.uv.y + 1) / 2;

				float depth = 1 - tex2D(_LastCameraDepthTexture, float2(0.0f, uvCoord)) * 1100;

				if (i.uv.x > depth * _height)
					depth = 0;
				else
					depth = 1;

				return depth;

				//return float4(uvCoord, 0, 0, 1);
				//return float4(1, 1, 1, tex2D(_LastCameraDepthTexture, float2(0, i.uv.x)));

				float fallOff = pow(clamp(1 - length(i.uv) / 50, 0, 1), 2);
				float sides = pow(dot(float2(1, 0), normalize(float2(i.uv.x, i.uv.y * 2))), 2);

				return float4(1, 1, 1, sides * fallOff);
			}
			ENDCG
		}
	}
}