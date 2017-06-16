﻿Shader "custom/fx_underwater"
{
	Properties
	{
		_Scene ("Scene", 2D) = "white" {}
		_Caustics ("Caustics", 2D) = "white" {}
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
			uniform sampler2D _CameraDepthNormalsTexture;

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
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv), linearDepth, viewNormal);

				// fragments world position
				float3 worldPos = worldPosition (linearDepth, i.ray);
				// fragments fake xy plane position
				float3 xyPlanePos = xyPlanePosition(i.ray);


				// SONAR COLORS
				half4 pulseCol = pulseColor(worldPos, i.uv);
				half4 pulseEdge = edgeCol(xyPlanePos);

				// PING COLORS
				half4 pingInterCol = pingInterColor(xyPlanePos);
				half4 pingHostileCol = pingHostileColor(xyPlanePos);

				// PULSES MASKING
				if(worldPos.z < 0) {
					pulseEdge = 0;
					pingInterCol = 0;
					pingHostileCol = 0;
				}
				
				// CAUSTICS COLOR
				half4 caustics = Caustics(worldPos);
				float cDiff = causticsDepthBlend(worldPos);
				float cMask = causticsMask(viewNormal, float3(0,1,0));
				caustics *= cDiff;
				caustics *= cMask;
				if(linearDepth > 0.9)
					caustics = 0;

				// FOG COLOR
				half4 fog = fogColor(worldPos);
				float fogDiff = fogBlend(linearDepth);

				//FINAL OUTPUT BLENDING
//				scene += caustics;
				scene = lerp(scene, fog, fogDiff);
//				scene = scene + pulseCol + pulseEdge;
//				scene += pingInterCol;
//				scene += pingHostileCol;

				return scene;
			}
			ENDCG
		}
	}
}