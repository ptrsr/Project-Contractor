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


			uniform sampler2D _LastCameraDepthTexture;


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}


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
				Cull Off ZTest Off
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


			uniform sampler2D _LastCameraDepthTexture;


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}


			uniform float _height;


			fixed4 frag(v2f i) : SV_Target
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
	}
}