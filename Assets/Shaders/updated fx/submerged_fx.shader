Shader "custom/submerged_fx"
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
			#include "Caustics.cginc"
			#include "Fog.cginc"
			#include "Sonar.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			uniform sampler2D _Scene;
			uniform sampler2D _CameraDepthTexture;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.ray = v.ray;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// scene render
				fixed4 scene = tex2D(_Scene, i.uv);

				// depth sampling
				float rawDepth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));
				float linearDepth = Linear01Depth(rawDepth);
				float eyeDepth = LinearEyeDepth(rawDepth);

				// fragments world position
				float3 worldPos = worldPosition (linearDepth, i.ray);
				// fragments fake xy plane position
				float3 xPlanePos = xPlanePosition(i.ray);

				// pulse colors
				half4 pulseCol = pulseColor(worldPos);
				half4 pulseEdge = edgeColor(xPlanePos);
				if(worldPos.z < 0)
					pulseEdge = 0;
				
				// caustics color
				half4 caustics = Caustics(worldPos);
				if(linearDepth > 0.9)
					caustics = 0;
				
				// fog color
				half4 fog = fogColor(i.uv);
				float fogDiff = fogBlend(eyeDepth);

			
				//final output blending
				//scene += caustics;
				scene = lerp(scene, fog, fogDiff);
				scene = scene + pulseCol + pulseEdge;

				return scene;
			}
			ENDCG
		}
	}
}