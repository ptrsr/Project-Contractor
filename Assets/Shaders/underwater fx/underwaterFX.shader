Shader "custom/fx_underwater"
{
	Properties
	{
		_Scene ("Scene", 2D) = "white" {}
		_Caustics ("Caustics", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Tags {"RenderType" = "Transparent"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Caustics_CG.cginc"
			#include "Fog_CG.cginc"
			#include "Sonar_CG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			uniform sampler2D _scene;
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
				fixed4 scene = tex2D(_scene, i.uv);

				// depth sampling
				float linearDepth;
				float3 viewNormal;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv), linearDepth, viewNormal);

				// fragments world position
				float3 worldPos = worldPosition (linearDepth, i.ray);
				// fragments fake xy plane position
				float3 xyPlanePos = xyPlanePosition(i.ray);

				// pulse colors
				half4 pulseCol = pulseColor(worldPos);
				half4 pulseEdge = edgeColor(xyPlanePos);
				if(worldPos.z < 0)
					pulseEdge = 0;
				
				// caustics color
				half4 caustics = Caustics(worldPos);
				float cDiff = causticsDepthBlend(worldPos);
				float cMask = causticsMask(viewNormal, float3(0,1,0));
				caustics *= cDiff;
				caustics *= cMask;
				if(linearDepth > 0.9)
					caustics = 0;
				
				// fog color
				half4 fog = fogColor(worldPos);
				float fogDiff = fogBlend(linearDepth);

				//final output blending
				scene += caustics;
				scene = lerp(scene, fog, fogDiff);
				scene = scene + pulseCol + pulseEdge;

				return scene;
			}
			ENDCG
		}
	}
}