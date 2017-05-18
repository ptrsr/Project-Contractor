Shader "custom/volumetric_fx"
{
	SubShader
	{
		Cull Off ZTest Always
		Tags {"RenderType" = "Transparent"}

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
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
				return Linear01Depth(tex2D(_CameraDepthTexture, float2(i.uv.x, 0)));
			}
			ENDCG
		}
	}
}