﻿Shader "custom/fx_underwater"
{
	Properties
	{
		_Scene ("Scene", 2D) = "white" {}
		_Caustics ("Caustics", 2D) = "white" {}
		_Grid("Grid", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
//		Tags{"Queue" = "Transparent+1"}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Caustics.cginc"
			#include "Fog.cginc"
			#include "Sonar.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv	  : TEXCOORD0;
				float4 ray	  : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv	  : TEXCOORD0;
				float4 ray	  : TEXCOORD1;
			};

			uniform sampler2D _Scene;
//			uniform sampler2D _CameraDepthNormalsTexture;
			uniform sampler2D _CameraDepthTexture;
			uniform sampler2D _Grid;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.uv = v.uv;
				o.ray = v.ray;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// scene render
				fixed4 scene = tex2D(_Scene, i.uv);

				// depth sampling
				float linearDepth;
				float3 viewNormal;

				linearDepth = Linear01Depth(tex2D(_CameraDepthTexture, i.uv));
//				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv), linearDepth, viewNormal);

				// fragments world position
				float3 worldPos = worldPosition (linearDepth, i.ray);
				// fragments fake xy plane position
				float3 xyPlanePos = xyPlanePosition(i.ray);


				// PULSE COLORS
				half4 outlineColor = pulseOutline(xyPlanePos);
				half4 highlightColor = pulseHighlight(worldPos);
				half4 paul = pulseForPaul(worldPos);

				// PING COLORS
				half4 pingColor = pulsePing(xyPlanePos);

				// PULSES MASKING
				if(worldPos.z < 0) {
					outlineColor = 0;
					pingColor = 0;
				}
				
				// CAUSTICS COLOR
				half4 caustics = Caustics(worldPos);
				float cDiff = causticsDepthBlend(worldPos);
//				float cMask = causticsMask(viewNormal, float3(0,1,0));
				caustics *= cDiff;
//				caustics *= cMask;
				if(linearDepth > 0.9)
					caustics = 0;


				// FOG COLOR
				half4 fog = fogColor(worldPos);
				float fogDiff = fogBlend(linearDepth);


				//final output blending
//				scene += caustics;
//				scene += paul;
				scene = lerp(scene, fog, fogDiff);
				scene += outlineColor + highlightColor;
				scene += pingColor;
//				return float(linearDepth);

				return scene;
			}
			ENDCG
		}
	}
}