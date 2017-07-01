﻿Shader "cs/depth"
{
	SubShader
	{

		Pass
		{
			Cull Off ZWrite Off ZTest Always
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

			uniform sampler2D _CameraDepthTexture;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float linearDepth = Linear01Depth (tex2D(_CameraDepthTexture, i.uv) );
				float eyeDepth = LinearEyeDepth (tex2D(_CameraDepthTexture, i.uv) );

				return linearDepth;
			}
			ENDCG
		}
	}
}
